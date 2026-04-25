using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Repository;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Game.Gameplay
{
    public class SaveManager : ISaveManager
    {
        private const string LastSavedVersionKey = "lastSavedVersion";
        
        private readonly ISaveSerializer[] _saveSerializers;
        private readonly IRepository _repository;

        public SaveManager(
            ISaveSerializer[] saveSerializers, 
            IRepository repository)
        {
            _saveSerializers = saveSerializers;
            _repository = repository;
        }
        
        public async UniTask<bool> Save(Action<bool, int> onSaved = null, CancellationToken cancellationToken = default)
        {
            var data = new JObject();
            var version = PlayerPrefs.GetInt(LastSavedVersionKey, 0) + 1;

            foreach (ISaveSerializer serializer in _saveSerializers)
                data.Add(serializer.Key, serializer.Serialize());
            
            bool success = await _repository.Save(version.ToString(), data, cancellationToken);

            if (!success)
                return false;
            
            PlayerPrefs.SetInt(LastSavedVersionKey, version);
            onSaved?.Invoke(true, version);

            return true;
        }

        public async UniTask<bool> Load(int version, Action<bool, int> onLoaded = null, CancellationToken cancellationToken = default)
        {
            (bool success, JObject data) = await _repository.Load(version.ToString(), cancellationToken);

            if (!success)
                return false;
            
            foreach (ISaveSerializer serializer in _saveSerializers)
            {
                if (data.TryGetValue(serializer.Key, out JToken value))
                    serializer.Deserialize(value);
            }
            
            onLoaded?.Invoke(true, version);
            return true;
        }
    }
}
