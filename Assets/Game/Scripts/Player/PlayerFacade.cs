using Game.Bullet;
using Game.Common;
using Game.Data;
using Game.Interfaces;
using UnityEngine;

namespace Game.Player
{
    public class PlayerFacade : MonoBehaviour, IAttackTarget, IDamageable
    {
        private HealthComponent _health;

        public Transform Transform => transform;
        public bool IsDead => _health.IsDead;
        public TeamType Team => TeamType.Player;

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
