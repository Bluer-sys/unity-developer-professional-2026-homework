using Game.GameObjects;
using UnityEngine;

namespace Game.GameContext
{
    public class EnemyFactory : MonoBehaviour, IFactory<Enemy>
    {
        [SerializeField] private Enemy _prefab;
        [SerializeField] private BulletSpawner _bulletSpawner;
        [SerializeField] private ShipViewConfig _viewConfig;
        [SerializeField] private ShipConfig _config;
        [SerializeField] private BulletConfig _bulletConfig;
        
        public Enemy Create(Vector3 position, Quaternion rotation, Transform parent)
        {
            var enemy = Instantiate(_prefab, position, rotation, parent);
            
            var weapon = enemy.GetComponent<IWeapon>();
            weapon.Construct(_bulletSpawner);
            weapon.ResetCooldown(_config.FireCooldown);
            
            enemy.SetWeapon(weapon);
            
            enemy.GetComponent<ShipMaterial>().SetMaterial(_viewConfig.MaterialPrefab);
            enemy.GetComponent<HealthComponent>().ResetHealth(_config.Health);
            enemy.GetComponent<MovementComponent>().SetSpeed(_config.MoveSpeed);
            
            return enemy;
        }
    }
}
