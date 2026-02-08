using System;
using Game.Common;
using Game.Data;
using Game.Interfaces;
using UnityEngine;

namespace Game.Enemy
{
    public sealed class EnemyFacade : MonoBehaviour, IDamageable
    {
        private HealthComponent _health;
        private EnemyBehaviour _behaviour;
        
        public event Action<EnemyFacade> OnDead;

        public TeamType Team { get; private set; }

        
        public void Construct(TeamType team, HealthComponent health, EnemyBehaviour behaviour)
        {
            Team = team;
            _health = health;
            _behaviour = behaviour;
            
            _health.OnDead += OnDeadHandler;
        }

        private void OnDestroy()
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

        public void ResetHealth()
        {
            _health.ResetHealth();
        }
    }
}
