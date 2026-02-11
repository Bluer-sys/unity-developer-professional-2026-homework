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
            _collisionHandler.OnTriggerEntered += OnTriggerEntered;
        }

        private void OnDisable()
        {
            _collisionHandler.OnTriggerEntered -= OnTriggerEntered;
        }

        public void SetDamage(int damage)
        {
            _damage = damage;
        }

        private void OnTriggerEntered(Collider2D other)
        {
            if (!other.TryGetComponent(out IDamageable damageable))
                return;

            damageable.TakeDamage(_damage);
        }
    }
}
