using Game.GameContext.Core;
using Game.GameObjects.Bullet;
using Modules.Utils;
using UnityEngine;

namespace Game.GameContext.Bullet
{
    public class BulletFactory : MonoBehaviour, IFactory<BulletBuilder>
    {
        [SerializeField] private BulletBuilder _prefab;
        [SerializeField] private TransformBounds _bounds;

        public BulletBuilder Create(Vector3 position, Quaternion rotation, Transform parent)
        {
            var builder = Instantiate(_prefab, position, rotation, parent)
                .Construct(_bounds);

            return builder;
        }
    }
}
