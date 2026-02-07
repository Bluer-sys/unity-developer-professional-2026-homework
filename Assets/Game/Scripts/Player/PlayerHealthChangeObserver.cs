using Game.Common;
using Modules.UI;
using Modules.Utils;
using UnityEngine;

namespace Game.Player
{
    public class PlayerHealthChangeObserver : MonoBehaviour
    {
        [SerializeField] private HealthComponent _health;
        [SerializeField] private CameraShaker _cameraShaker;
        [SerializeField] private HealthView _healthView;

        private void OnEnable()
        {
            _health.OnHealthChanged += OnHealthChanged;
        }

        private void OnDisable()
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
