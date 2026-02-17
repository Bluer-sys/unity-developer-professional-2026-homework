using System;
using Game.GameObjects.Core;
using Modules.Utils;
using UnityEngine;

namespace Game.GameObjects.Bullet
{
    public sealed class Bullet : MonoBehaviour
    {
        private CollisionHandler _collisionHandler;
        private TransformBounds _bounds;
        
        private int _damage;
        
        public event Action OnDead;

        public void Construct(TransformBounds bounds, 
                              CollisionHandler collisionHandler)
        {
            _bounds = bounds;
            _collisionHandler = collisionHandler;
            
            _collisionHandler.OnTriggerEntered += OnTriggerEntered;
        }
        
        private void OnDestroy()
        {
            _collisionHandler.OnTriggerEntered -= OnTriggerEntered;
        }

        public void FixedUpdate()
        {
            if (!_bounds.InBounds(transform.position))
                Dead();
        }

        public void SetDamage(int damage)
        {
            _damage = damage;
        }

        private void OnTriggerEntered(Collider2D other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
                damageable.TakeDamage(_damage);
                
            Dead();
        }

        private void Dead()
        {
            OnDead?.Invoke();
        }
    }
}
