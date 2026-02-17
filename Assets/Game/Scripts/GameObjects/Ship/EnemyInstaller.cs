using Game.GameContext.Bullet;
using Game.GameObjects.Bullet;
using Game.GameObjects.Core;
using UnityEngine;

namespace Game.GameObjects.Ship
{
    public class EnemyInstaller : MonoBehaviour
    {
        [SerializeField] private ShipViewConfig _viewConfig;
        [SerializeField] private ShipConfig _config;
        [SerializeField] private BulletConfig _bulletConfig;
        
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private MovementComponent _movement;
        [SerializeField] private HealthComponent _health;
        [SerializeField] private Weapon.CooldownWeapon _weapon;
        [SerializeField] private ShipMaterial _shipMaterial;
        [SerializeField] private AudioSource _audioSource;

        [SerializeField] private DeathViewComponent _deathViewComponent;
        [SerializeField] private TakeDamageViewComponent _takeDamageViewComponent;
        [SerializeField] private MovementViewComponent _movementViewComponent;

        [SerializeField] private EnemyFacade _enemyFacade;
        [SerializeField] private EnemyBehaviour _enemyBehaviour;
        
        public EnemyFacade Install(BulletSpawner bulletSpawner)
        {
            _shipMaterial.SetMaterial(_viewConfig.MaterialPrefab);
            
            _weapon.Construct(bulletSpawner, _bulletConfig, _config.FireCooldown);

            _health.Construct(_config.Health);
            _movement.Construct(_rigidbody, _config.MoveSpeed);
            
            _takeDamageViewComponent.Construct(_health, _viewConfig, _viewConfig.DamageSfx, _audioSource, _shipMaterial);
            _movementViewComponent.Construct(_movement, _viewConfig.MoveRotationAngle, _viewConfig.MoveSpeed);
            _deathViewComponent.Construct(_health, _viewConfig);
            
            _enemyBehaviour.Construct(_movement, _weapon, _config.StoppingDistance);
            _enemyFacade.Construct(_health, _enemyBehaviour);
            
            return _enemyFacade;
        }
    }
}
