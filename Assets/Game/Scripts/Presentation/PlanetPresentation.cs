using System;
using Modules.Planets;
using R3;
using UnityEngine;
using Zenject;

namespace Game.Presentation
{
    public class PlanetPresentation : IInitializable, IDisposable
    {
        public string Price => _planet.Price.ToString();
        public Sprite Sprite => _planet.GetIcon(_planet.IsUnlocked);
        public ReadOnlyReactiveProperty<bool> IsUnlocked => _isUnlocked;
        public ReadOnlyReactiveProperty<string> IncomeRemainingTime => _incomeRemainingTime;
        public ReadOnlyReactiveProperty<float> IncomeProgress => _incomeProgress;
        public ReadOnlyReactiveProperty<bool> IsIncomeReady => _isIncomeReady;

        private readonly ReactiveProperty<bool> _isUnlocked = new();
        private readonly ReactiveProperty<string> _incomeRemainingTime = new();
        private readonly ReactiveProperty<float> _incomeProgress = new();
        private readonly ReactiveProperty<bool> _isIncomeReady = new();
        
        private readonly PlanetPopupPresentation _popupPresentation;
        private readonly IPlanet _planet;

        public PlanetPresentation(IPlanet planet, PlanetPopupPresentation popupPresentation)
        {
            _planet = planet;
            _popupPresentation = popupPresentation;
        }

        public void Initialize()
        {
            _planet.OnUnlocked += OnUnlocked;
            _planet.OnIncomeTimeChanged += OnIncomeTimeChanged;
            _planet.OnIncomeReady += OnIncomeReady;
            
            OnUnlocked();
        }

        public void Dispose()
        {
            _planet.OnUnlocked -= OnUnlocked;
            _planet.OnIncomeTimeChanged -= OnIncomeTimeChanged;
            _planet.OnIncomeReady -= OnIncomeReady;
        }

        public void OnPlanetClicked()
        {
            if (!_planet.IsUnlocked && _planet.CanUnlock)
                _planet.Unlock();

            if (_planet.IsUnlocked)
                _planet.GatherIncome();
        }

        public void OnPlanetPopupRequested()
        {
            if(_planet.IsUnlocked)
                _popupPresentation.Show(_planet);
        }

        private void OnIncomeReady(bool isReady)
        {
            _isIncomeReady.Value = isReady;
        }

        private void OnIncomeTimeChanged(float value)
        {
            _incomeRemainingTime.Value = $"{Mathf.RoundToInt(value / 60)}m:{Mathf.RoundToInt(value % 60)}s";
            _incomeProgress.Value = _planet.IncomeProgress;
        }

        private void OnUnlocked()
        {
            _isUnlocked.Value = _planet.IsUnlocked;
        }
    }
}
