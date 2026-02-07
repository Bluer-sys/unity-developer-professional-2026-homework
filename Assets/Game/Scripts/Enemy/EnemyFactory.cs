using Game.Bullet;
using Game.Installers;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyFactory : MonoBehaviour
    {
        [SerializeField] private EnemyInstaller _prefab;
        [SerializeField] private Transform _container;
        [SerializeField] private BulletWorldGo _bulletWorldGo;
        
        public EnemyFacade Create(Vector3 position)
        {
            EnemyInstaller installer = Instantiate(_prefab, position, Quaternion.identity, _container);
            EnemyFacade enemy = installer.Install(_bulletWorldGo);
            
            return enemy;
        }
    }
}
