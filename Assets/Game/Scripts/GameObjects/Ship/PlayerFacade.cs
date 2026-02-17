using Game.GameObjects.Core;
using UnityEngine;

namespace Game.GameObjects.Ship
{
    public class PlayerFacade : MonoBehaviour, IAttackTarget, IDamageable
    {
        private HealthComponent _health;

        public Transform Transform => transform;
        public bool IsDead => _health.IsDead;

        public void Construct(HealthComponent health)
        {
            _health = health;
        }

        public void TakeDamage(int damage)
        {
            _health.Decrease(damage);
        }
    }
}
