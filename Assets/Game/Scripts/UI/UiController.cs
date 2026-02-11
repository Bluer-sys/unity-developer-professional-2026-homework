using Game.Common;
using Game.Enemy;
using Modules.UI;
using UnityEngine;

namespace Game.UI
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private ScoreView _scoreView;
        [SerializeField] private GameOverView _gameOverView;
        [SerializeField] private HealthView _healthView;
        
        [SerializeField] private HealthComponent _health;
        [SerializeField] private EnemySpawner _enemySpawner;
        
        private void Awake()
        {
            _health.OnHealthChanged += OnHealthChanged;
            _health.OnDead += _gameOverView.Show;
            _enemySpawner.OnEnemyDead += OnEnemyDead;
            
            _scoreView.SetValue(0);
        }

        private void OnDestroy()
        {
            _health.OnHealthChanged -= OnHealthChanged;
            _health.OnDead -= _gameOverView.Show;
            _enemySpawner.OnEnemyDead -= OnEnemyDead;
        }

        private void OnHealthChanged(int current)
        {
            _healthView.SetHealth(current, _health.MaxHealth);
        }

        private void OnEnemyDead()
        {
            _scoreView.SetValue(_enemySpawner.DestroyedEnemies);
        }
    }
}
