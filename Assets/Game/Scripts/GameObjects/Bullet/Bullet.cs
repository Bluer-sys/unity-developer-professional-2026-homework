using System;
using Modules.Utils;
using UnityEngine;

namespace Game.GameObjects
{
    public sealed class Bullet : MonoBehaviour
    {
        [SerializeField] private CollisionHandler _collisionHandler;
        
        private TransformBounds _bounds;
        private int _damage;
        
        public event Action<Bullet> OnDead;

        public void Construct(TransformBounds bounds)
        {
            _bounds = bounds;
            
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
            OnDead?.Invoke(this);
        }
    }
}
