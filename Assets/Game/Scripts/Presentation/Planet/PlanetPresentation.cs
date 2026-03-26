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

        private readonly IPlanet _planet;
        private readonly SignalBus _signalBus;

        private Vector3 _coinPosition;
        
        public PlanetPresentation(IPlanet planet, SignalBus signalBus)
        {
            _planet = planet;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _planet.OnUnlocked += OnUnlocked;
            _planet.OnIncomeTimeChanged += OnIncomeTimeChanged;
            _planet.OnIncomeReady += OnIncomeReady;
            _planet.OnGathered += OnGathered;
            
            OnUnlocked();
            OnIncomeReady(false);
        }

        public void Dispose()
        {
            _planet.OnUnlocked -= OnUnlocked;
            _planet.OnIncomeTimeChanged -= OnIncomeTimeChanged;
            _planet.OnIncomeReady -= OnIncomeReady;
            _planet.OnGathered -= OnGathered;
        }

        public void OnCoinPositionSet(Vector3 position) =>
            _coinPosition = position;

        public void OnPlanetClicked()
        {
            if (!_planet.IsUnlocked && _planet.CanUnlock)
            {
                _planet.Unlock();
                return;
            }

            if (_planet.IsUnlocked && _planet.IsIncomeReady)
                _planet.GatherIncome();
        }

        public void OnPlanetPopupRequested()
        {
            if (_planet.IsUnlocked)
                _signalBus.Fire(new OnPlanetPopupRequestedSignal(_planet));
        }

        private void OnGathered(int range) =>
            _signalBus.Fire(new OnMoneyEarnedSignal(_coinPosition, range));

        private void OnIncomeReady(bool isReady) =>
            _isIncomeReady.Value = isReady;

        private void OnUnlocked() =>
            _isUnlocked.Value = _planet.IsUnlocked;

        private void OnIncomeTimeChanged(float value)
        {
            _incomeRemainingTime.Value = $"{Mathf.RoundToInt(value / 60)}m:{Mathf.RoundToInt(value % 60)}s";
            _incomeProgress.Value = _planet.IncomeProgress;
        }
    }
}
