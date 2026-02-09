using Game.Bullet;
using Game.Common;
using Game.Data;
using Game.Enemy;
using Game.Weapon;
using UnityEngine;

namespace Game.Installers
{
    public class EnemyInstaller : MonoBehaviour
    {
        [SerializeField] private ShipViewConfig _viewConfig;
        [SerializeField] private ShipConfig _config;
        [SerializeField] private BulletConfig _bulletConfig;
        
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private MovementComponent _movement;
        [SerializeField] private HealthComponent _health;
        [SerializeField] private CooldownWeapon _weapon;
        [SerializeField] private ShipMaterial _shipMaterial;
        [SerializeField] private AudioSource _audioSource;

        [SerializeField] private DeathEffectsController _deathEffectsController;
        [SerializeField] private TakeDamageEffectController _takeDamageEffectController;
        [SerializeField] private MovementAnimator _movementAnimator;

        [SerializeField] private EnemyFacade _enemyFacade;
        [SerializeField] private EnemyBehaviour _enemyBehaviour;
        
        public EnemyFacade Install(BulletSpawner bulletSpawner)
        {
            _shipMaterial.SetMaterial(_viewConfig.MaterialPrefab);
            
            _weapon.Construct(bulletSpawner, _config.FireCooldown, _bulletConfig);

            _health.Construct(_config.Health);
            _movement.Construct(_rigidbody, _config.MoveSpeed);
            
            _takeDamageEffectController.Construct(_health, _viewConfig, _viewConfig.DamageSfx, _audioSource, _shipMaterial);
            _movementAnimator.Construct(_movement, _viewConfig.MoveRotationAngle, _viewConfig.MoveSpeed);
            _deathEffectsController.Construct(_health, _viewConfig);
            
            _enemyBehaviour.Construct(_movement, _weapon, _config.StoppingDistance);
            _enemyFacade.Construct(_health, _enemyBehaviour);
            
            return _enemyFacade;
        }
    }
}
