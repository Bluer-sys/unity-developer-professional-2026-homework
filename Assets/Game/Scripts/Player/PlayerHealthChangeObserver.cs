using Game.Common;
using Modules.UI;
using Modules.Utils;
using UnityEngine;

namespace Game.Player
{
    public class PlayerHealthChangeObserver : MonoBehaviour
    {
        private HealthComponent _health;
        private CameraShaker _cameraShaker;
        private HealthView _healthView;

        public void Construct(HealthComponent health, CameraShaker cameraShaker, HealthView healthView)
        {
            _health = health;
            _cameraShaker = cameraShaker;
            _healthView = healthView;
            
            _health.OnHealthChanged += OnHealthChanged;
        }

        private void OnDestroy()
        {
            _health.OnHealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(int health)
        {
            _healthView.SetHealth(health, _health.MaxHealth);
            _cameraShaker.Shake();
        }
    }
}
