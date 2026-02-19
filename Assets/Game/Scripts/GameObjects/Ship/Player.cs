using UnityEngine;

namespace Game.GameObjects
{
    public class Player : MonoBehaviour, IAttackTarget
    {
        [SerializeField] private ShipConfig _config;
        [SerializeField] private ShipViewConfig _viewConfig;
        [SerializeField] private Weapon _weapon;
        [SerializeField] private HealthComponent _health;
        [SerializeField] private MovementComponent _movement;
        [SerializeField] private ShipMaterial _shipMaterial;

        public Transform Transform => transform;
        public bool IsDead => _health.IsDead;

        private void Awake()
        {
            _weapon.ResetCooldown(_config.FireCooldown);
            _health.ResetHealth(_config.Health);
            _movement.SetSpeed(_config.MoveSpeed);
            _shipMaterial.SetMaterial(_viewConfig.MaterialPrefab);
        }
    }
}
