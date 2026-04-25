using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Game.Repository
{
    public sealed class SyncRepository : IRepository
    {
        private readonly IRepository[] _repositories;

        public SyncRepository(params IRepository[] repositories)
        {
            _repositories = repositories;
        }

        public async UniTask<bool> Save(string version, JObject data, CancellationToken ct = default)
        {
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            data["saveTime"] = timestamp;

            int count = _repositories.Length;
            if (count == 0)
                return false;

            UniTask<bool>[] tasks = new UniTask<bool>[count];
            for (int i = 0; i < count; i++) 
                tasks[i] = _repositories[i].Save(version, data, ct);

            bool[] results = await UniTask.WhenAll(tasks);
            return results.Any(x => x);
        }

        public async UniTask<(bool, JObject)> Load(string version, CancellationToken ct = default)
        {
            int count = _repositories.Length;
            if (count == 0)
                return (false, null);
            
            UniTask<(bool, JObject)>[] tasks = new UniTask<(bool, JObject)>[count];
            for (int i = 0; i < count; i++)
                tasks[i] = _repositories[i].Load(version, ct);

            (bool, JObject)[] results = await UniTask.WhenAll(tasks);

            long lastTimestamp = long.MinValue;
            JObject lastData = null;

            for (int i = 0; i < count; i++)
            {
                (bool success, JObject gameData) = results[i];
                if (!success || gameData == null)
                    continue;

                long timestamp = 0;
                if (gameData.TryGetValue("saveTime", out JToken token)) 
                    timestamp = token.Value<long>();

                if (timestamp > lastTimestamp)
                {
                    lastTimestamp = timestamp;
                    lastData = gameData;
                }
            }

            return lastData != null ? (true, lastData) : (false, null);
        }
    }
}
