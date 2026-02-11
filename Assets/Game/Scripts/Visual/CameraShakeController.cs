using Game.Common;
using Modules.Utils;
using UnityEngine;

namespace Game.Visual
{
    public class CameraShakeController : MonoBehaviour
    {
        [SerializeField] private HealthComponent _health;
        [SerializeField] private CameraShaker _cameraShaker;

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
            _cameraShaker.Shake();
        }
    }
}
