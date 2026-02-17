using UnityEngine;

namespace Game.GameContext.Core
{
    public interface IFactory<out TObject>
    {
        TObject Create(Vector3 position, Quaternion rotation, Transform parent);
    }
}
