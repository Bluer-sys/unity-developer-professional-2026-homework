using Game.Bullet;
using Game.Installers;
using Modules.Utils;
using UnityEngine;

namespace Game.Factory
{
    public class BulletFactory : MonoBehaviour, IFactory<BulletBuilder>
    {
        [SerializeField] private BulletInstaller _prefab;
        [SerializeField] private Transform _container;
        [SerializeField] private TransformBounds _bounds;

        public BulletBuilder Create(Vector3 position, Quaternion rotation)
        {
            var installer = Instantiate(_prefab, position, rotation, _container);
            var builder = installer.Install(_bounds);

            return builder;
        }
    }
}
