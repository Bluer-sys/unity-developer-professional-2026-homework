using Game.Bullet;
using Game.Common;
using Game.Data;
using Game.Player;
using Game.Weapon;
using Modules.Utils;
using UnityEngine;

namespace Game.Installers
{
    public class PlayerInstaller : MonoBehaviour
    {
        [SerializeField] private ShipViewConfig _viewConfig;
        [SerializeField] private ShipConfig _config;
        [SerializeField] private BulletConfig _bulletConfig;
        
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private MovementComponent _movement;
        [SerializeField] private HealthComponent _health;
        [SerializeField] private CooldownWeapon _weapon;
        [SerializeField] private BulletWorldGo _bulletWorld;
        [SerializeField] private CameraShaker _cameraShaker;
        [SerializeField] private TransformBounds _bounds;
        [SerializeField] private ShipMaterial _shipMaterial;
        [SerializeField] private AudioSource _audioSource;
        
        [SerializeField] private DeathEffectsController _deathEffectsController;
        [SerializeField] private TakeDamageEffectController _takeDamageEffectController;
        [SerializeField] private MovementAnimator _movementAnimator;
        
        [SerializeField] private PlayerFacade _playerFacade;
        [SerializeField] private PlayerFireController _fireController;
        [SerializeField] private PlayerMovementController _movementController;
        [SerializeField] private PlayerDeathObserver _playerDeathObserver;

        private TeamType Team => TeamType.Player;
        
        private void Awake()
        {
            _shipMaterial.SetMaterial(_viewConfig.MaterialPrefab);
            
            _weapon.Construct(Team, _bulletWorld, _config.FireCooldown, _bulletConfig);
            
            _health.Construct(_config.Health);
            _movement.Construct(_rigidbody, _bounds, _config.MoveSpeed);
            
            _takeDamageEffectController.Construct(_health, _viewConfig, _viewConfig.DamageSfx, _audioSource, _shipMaterial);
            _movementAnimator.Construct(_movement, _viewConfig.MoveRotationAngle, _viewConfig.MoveSpeed);
            _deathEffectsController.Construct(_health, _viewConfig);
            
            _playerFacade.Construct(Team, _health);
            _fireController.Construct(_weapon);
            _movementController.Construct(_movement);
            _playerDeathObserver.Construct(_health);
        }
    }
}
