using Game.Common;
using Modules.UI;
using UnityEngine;

namespace Game.UI
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private HealthComponent _health;
        [SerializeField] private GameOverView _gameOverView;
        [SerializeField] private HealthView _healthView;
        
        private void Awake()
        {
            _health.OnHealthChanged += OnHealthChanged;
            _health.OnDead += _gameOverView.Show;
        }

        private void OnDestroy()
        {
            _health.OnHealthChanged -= OnHealthChanged;
            _health.OnDead -= _gameOverView.Show;
        }

        private void OnHealthChanged(int current)
        {
            _healthView.SetHealth(current, _health.MaxHealth);
        }
    }
}
