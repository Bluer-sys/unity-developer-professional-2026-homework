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
        public TeamType Team { get; private set; }

        public void Construct(TeamType team, HealthComponent health)
        {
            _health = health;
            Team = team;
        }

        public void TakeDamage(int damage)
        {
            _health.Decrease(damage);
        }
    }
}
