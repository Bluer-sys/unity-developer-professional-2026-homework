using Game.GameObjects.Core;
using UnityEngine;

namespace Game.GameObjects.Ship
{
    public class Player : MonoBehaviour, IAttackTarget
    {
        private HealthComponent _health;

        public Transform Transform => transform;
        public bool IsDead => _health.IsDead;

        public void Construct(HealthComponent health)
        {
            _health = health;
        }
    }
}
