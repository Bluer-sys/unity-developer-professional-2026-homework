using Game.GameObjects.Core;
using UnityEngine;

namespace Game.GameContext.Player
{
    public class PlayerDeathObserver : MonoBehaviour
    {
        private HealthComponent _health;

        public void Construct(HealthComponent health)
        {
            _health = health;

            _health.OnDead += OnDead;
        }

        private void OnDestroy()
        {
            _health.OnDead -= OnDead;
        }

        private void OnDead()
        {
            gameObject.SetActive(false);
        }
    }
}
