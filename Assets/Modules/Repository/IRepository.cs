using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Game.Repository
{
    public interface IRepository
    {
        UniTask<bool> Save(string version, JObject data, CancellationToken cancellationToken = default);
        UniTask<(bool, JObject)> Load(string version, CancellationToken cancellationToken = default);
    }
}
