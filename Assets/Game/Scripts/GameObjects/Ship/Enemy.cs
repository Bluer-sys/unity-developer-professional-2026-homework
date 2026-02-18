using System;
using Game.GameObjects.Core;
using UnityEngine;

namespace Game.GameObjects.Ship
{
    public sealed class Enemy : MonoBehaviour
    {
        private HealthComponent _health;
        private EnemyBehaviour _behaviour;
        
        public event Action<Enemy> OnDead;
        
        public void Construct(HealthComponent health, EnemyBehaviour behaviour)
        {
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

        public void ResetHealth()
        {
            _health.ResetHealth();
        }
    }
}
