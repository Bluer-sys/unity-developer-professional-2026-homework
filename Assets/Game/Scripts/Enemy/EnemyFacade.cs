using System;
using Game.Bullet;
using Game.Common;
using Game.Data;
using Game.Interfaces;
using UnityEngine;

namespace Game.Enemy
{
    public sealed class EnemyFacade : MonoBehaviour, IDamageable
    {
        private HealthComponent _health;
        private MovementComponent _movement;
        private EnemyBehaviour _behaviour;
        
        public event Action<EnemyFacade> OnDead;
        
        public TeamType Team => TeamType.Enemy;
        
        public void Construct(HealthComponent health, MovementComponent movement, EnemyBehaviour behaviour)
        {
            _health = health;
            _movement = movement;
            _behaviour = behaviour;
        }

        private void OnEnable()
        {
            _health.OnDead += OnDeadHandler;
        }

        private void OnDisable()
        {
            _health.OnDead -= OnDeadHandler;
        }

        private void OnDeadHandler()
        {
            OnDead?.Invoke(this);
        }

        public void SetDestination(Vector3 destination)
        {
            _behaviour.SetDestination(destination);
        }

        public void SetTarget(IAttackTarget target)
        {
            _behaviour.SetTarget(target);
        }

        public void TakeDamage(int damage)
        {
            _health.Decrease(damage);
        }
    }
}
