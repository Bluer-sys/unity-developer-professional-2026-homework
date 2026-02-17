using Game.GameContext.Bullet;
using Game.GameContext.Player;
using Game.GameObjects.Bullet;
using Game.GameObjects.Core;
using Modules.Utils;
using UnityEngine;

namespace Game.GameObjects.Ship
{
    public class PlayerInstaller : MonoBehaviour
    {
        [SerializeField] private ShipViewConfig _viewConfig;
        [SerializeField] private ShipConfig _config;
        [SerializeField] private BulletConfig _bulletConfig;
        
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private MovementComponent _movement;
        [SerializeField] private HealthComponent _health;
        [SerializeField] private Weapon.Weapon _weapon;
        [SerializeField] private BulletSpawner _bulletWorld;
        [SerializeField] private CameraShaker _cameraShaker;
        [SerializeField] private TransformBounds _bounds;
        [SerializeField] private ShipMaterial _shipMaterial;
        [SerializeField] private AudioSource _audioSource;
        
        [SerializeField] private DeathViewComponent _deathViewComponent;
        [SerializeField] private TakeDamageViewComponent _takeDamageViewComponent;
        [SerializeField] private MovementViewComponent _movementViewComponent;
        
        [SerializeField] private PlayerFacade _playerFacade;
        [SerializeField] private PlayerFireController _fireController;
        [SerializeField] private PlayerMovementController _movementController;
        [SerializeField] private PlayerDeathObserver _playerDeathObserver;

        private void Awake()
        {
            _shipMaterial.SetMaterial(_viewConfig.MaterialPrefab);
            
            _weapon.Construct(_bulletWorld, _bulletConfig, _config.FireCooldown);
            
            _health.Construct(_config.Health);
            _movement.Construct(_rigidbody, _config.MoveSpeed);
            
            _takeDamageViewComponent.Construct(_health, _viewConfig, _viewConfig.DamageSfx, _audioSource, _shipMaterial);
            _movementViewComponent.Construct(_movement, _viewConfig.MoveRotationAngle, _viewConfig.MoveSpeed);
            _deathViewComponent.Construct(_health, _viewConfig);
            
            _playerFacade.Construct(_health);
            _fireController.Construct(_weapon);
            _movementController.Construct(_movement, _bounds);
            _playerDeathObserver.Construct(_health);
        }
    }
}
