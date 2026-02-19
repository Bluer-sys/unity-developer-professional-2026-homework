using System;
using UnityEngine;

namespace Game.GameObjects
{
    public sealed class Enemy : MonoBehaviour
    {
        [SerializeField] private ShipConfig _config;
        [SerializeField] private HealthComponent _health;
        [SerializeField] private MovementComponent _movement;

        private IWeapon _weapon;
        private IAttackTarget _target;
        private Vector2 _destination;

        public event Action<Enemy> OnDead;

        private void OnEnable()
        {
            _health.OnDead += DeadHandler;
        }

        private void OnDisable()
        {
            _health.OnDead -= DeadHandler;
        }

        private void FixedUpdate()
        {
            if (_target == null || _target.IsDead)
                return;

            Vector2 distance = _destination - (Vector2)transform.position;
            Vector2 moveDirection = distance.normalized;
            Vector2 fireDirection = (_target.Transform.position - transform.position).normalized;
            float stoppingDistance = _config.StoppingDistance;
            bool isNotReached = distance.sqrMagnitude > stoppingDistance * stoppingDistance;

            if (isNotReached)
            {
                _movement.SetDirection(moveDirection);
            }
            else
            {
                _movement.ResetDirection();
                _weapon.TryFire(fireDirection);
            }
        }

        public void SetTarget(IAttackTarget target)
        {
            _target = target;
        }

        public void SetDestination(Vector3 destination)
        {
            _destination = destination;
        }
        
        public void SetWeapon(IWeapon weapon)
        {
            _weapon = weapon;
        }
        
        public void ResetHealth()
        {
            _health.ResetHealth();
        }

        private void DeadHandler()
        {
            OnDead?.Invoke(this);
        }
    }
}
