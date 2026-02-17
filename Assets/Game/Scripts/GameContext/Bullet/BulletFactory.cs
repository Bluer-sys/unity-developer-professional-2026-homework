using Game.GameContext.Core;
using Game.GameObjects.Bullet;
using Modules.Utils;
using UnityEngine;

namespace Game.GameContext.Bullet
{
    public class BulletFactory : MonoBehaviour, IFactory<BulletBuilder>
    {
        [SerializeField] private BulletInstaller _prefab;
        [SerializeField] private TransformBounds _bounds;

        public BulletBuilder Create(Vector3 position, Quaternion rotation, Transform parent)
        {
            var installer = Instantiate(_prefab, position, rotation, parent);
            var builder = installer.Install(_bounds);

            return builder;
        }
    }
}
