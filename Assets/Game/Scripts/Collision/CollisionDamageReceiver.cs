using Game.Interfaces;
using UnityEngine;

namespace Game.Collision
{
    public class CollisionDamageReceiver : MonoBehaviour
    {
        [SerializeField] private CollisionHandler _collisionHandler;
        
        private int _damage;

        private void OnEnable()
        {
            _collisionHandler.OnTriggerEntered += OnTriggerEnter2D;
        }

        private void OnDisable()
        {
            _collisionHandler.OnTriggerEntered -= OnTriggerEnter2D;
        }

        public void SetDamage(int damage)
        {
            _damage = damage;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out IDamageable damageable))
                return;

            damageable.TakeDamage(_damage);
        }
    }
}
