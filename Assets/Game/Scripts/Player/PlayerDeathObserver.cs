using Game.Common;
using Modules.UI;
using UnityEngine;

namespace Game.Player
{
    public class PlayerDeathObserver : MonoBehaviour
    {
        [SerializeField] private HealthComponent _health;
        [SerializeField] private GameOverView _gameOverView;

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
            _gameOverView.Show();
        }
    }
}
