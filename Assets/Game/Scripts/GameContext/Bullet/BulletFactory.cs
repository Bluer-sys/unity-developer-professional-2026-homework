using Game.GameObjects;
using Modules.Utils;
using UnityEngine;

namespace Game.GameContext
{
    public class BulletFactory : MonoBehaviour, IFactory<Bullet>
    {
        [SerializeField] private Bullet _prefab;
        [SerializeField] private TransformBounds _bounds;

        public Bullet Create(Vector3 position, Quaternion rotation, Transform parent)
        {
            var bullet = Instantiate(_prefab, position, rotation, parent);
            bullet.Construct(_bounds);

            return bullet;
        }
    }
}
