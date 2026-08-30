using System;
using Game.Core;
using UnityEngine;

namespace Game
{
    public class PortalHealthPresenter : MonoBehaviour
    {
        [SerializeField] private HealthComponent _portalHealth;
        [SerializeField] private SmoothHealthBar _bar;

        private void OnEnable()
        {
            _portalHealth.OnHealthChanged += UpdateHealth;
        }

        private void OnDisable()
        {
            _portalHealth.OnHealthChanged -= UpdateHealth;
        }

        private void UpdateHealth(int value)
        {
            _bar.Set((float)value/_portalHealth.Max, false);
            _bar.SetCaption($"{value}/{_portalHealth.Max}");
        }
    }
}
