using Game.GameObjects.Core;
using UnityEngine;

namespace Game.GameContext.Player
{
    public class PlayerDeathObserver : MonoBehaviour
    {
        [SerializeField] private HealthComponent _health;
        [SerializeField] private GameObject _player;

        private void OnEnable()
        {
            _health.OnDead += OnDead;
        }

        private void OnDisable()
        {
            _health.OnDead -= OnDead;
        }

        private void OnDead()
        {
            _player.SetActive(false);
        }
    }
}
