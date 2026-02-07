using Game.Common;
using Modules.UI;
using UnityEngine;

namespace Game.Player
{
    public class PlayerDeathObserver : MonoBehaviour
    {
        private HealthComponent _health;
        private GameOverView _gameOverView;

        public void Construct(HealthComponent health, GameOverView gameOverView)
        {
            _health = health;
            _gameOverView = gameOverView;
            
            _health.OnDead += OnDead;
        }   
        
        private void OnDestroy()
        {
            _health.OnDead -= OnDead;
        }

        private void OnDead()
        {
            _gameOverView.Show();
        }
    }
}
