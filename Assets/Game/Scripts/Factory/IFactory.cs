using UnityEngine;

namespace Game.Factory
{
    public interface IFactory<out TObject>
    {
        TObject Create(Vector3 position, Quaternion rotation);
    }
}
