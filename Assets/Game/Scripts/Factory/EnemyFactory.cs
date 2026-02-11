using Game.Bullet;
using Game.Enemy;
using Game.Installers;
using UnityEngine;

namespace Game.Factory
{
    public class EnemyFactory : MonoBehaviour, IFactory<EnemyFacade>
    {
        [SerializeField] private EnemyInstaller _prefab;
        [SerializeField] private BulletSpawner _bulletSpawner;
        
        public EnemyFacade Create(Vector3 position, Quaternion rotation, Transform parent)
        {
            var installer = Instantiate(_prefab, position, rotation, parent);
            var enemy = installer.Install(_bulletSpawner);
            
            return enemy;
        }
    }
}
