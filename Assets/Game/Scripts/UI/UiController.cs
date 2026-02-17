using Game.GameContext.Enemy;
using Game.GameObjects.Core;
using Modules.UI;
using UnityEngine;

namespace Game.UI
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private ScoreView _scoreView;
        [SerializeField] private GameOverView _gameOverView;
        [SerializeField] private HealthView _healthView;
        
        [SerializeField] private GameObject _player;
        [SerializeField] private EnemySpawner _enemySpawner;

        private HealthComponent _playerHealth;
        
        private void Awake()
        {
            _playerHealth = _player.GetComponent<HealthComponent>();

            _playerHealth.OnHealthChanged += OnHealthChanged;
            _playerHealth.OnDead += _gameOverView.Show;
            _enemySpawner.OnEnemyDead += OnEnemyDead;
            
            _scoreView.SetValue(0);
        }

        private void OnDestroy()
        {
            _playerHealth.OnHealthChanged -= OnHealthChanged;
            _playerHealth.OnDead -= _gameOverView.Show;
            _enemySpawner.OnEnemyDead -= OnEnemyDead;
        }

        private void OnHealthChanged(int current)
        {
            _healthView.SetHealth(current, _playerHealth.MaxHealth);
        }

        private void OnEnemyDead()
        {
            _scoreView.SetValue(_enemySpawner.DestroyedEnemies);
        }
    }
}
