using Game.GameContext.Bullet;
using Game.GameContext.Core;
using Game.GameObjects.Ship;
using UnityEngine;

namespace Game.GameContext.Enemy
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
