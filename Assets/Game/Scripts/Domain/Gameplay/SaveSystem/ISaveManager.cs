using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Game.Gameplay
{
    public interface ISaveManager
    {
        UniTask<bool> Save(
            Action<bool, int> onSaved = null,
            CancellationToken cancellationToken = default);
        
        UniTask<bool> Load(
            int version,
            Action<bool, int> onLoaded = null, 
            CancellationToken cancellationToken = default);
    }
}
