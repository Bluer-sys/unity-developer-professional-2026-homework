using System;
using Modules.Planets;
using UnityEngine;
using Zenject;

namespace Game.UI
{
    public class PlanetPresenter : MonoBehaviour, IInitializable, IDisposable
    {
        [SerializeField] private PlanetView _view;

        private SignalBus _signalBus;
        private IPlanet _planet;

        public void Construct(IPlanet planet, SignalBus signalBus)
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

            _view.OnClick += OnClicked;
            _view.OnHold += OnPopupRequested;

            SetUnlocked(false);
            OnIncomeReady(false);
        }

        public void Dispose()
        {
            _planet.OnUnlocked -= OnUnlocked;
            _planet.OnIncomeTimeChanged -= OnIncomeTimeChanged;
            _planet.OnIncomeReady -= OnIncomeReady;
            _planet.OnGathered -= OnGathered;

            _view.OnClick -= OnClicked;
            _view.OnHold -= OnPopupRequested;
        }

        private void OnClicked()
        {
            if (!_planet.IsUnlocked && _planet.CanUnlock)
            {
                _planet.Unlock();
                return;
            }

            if (_planet.IsUnlocked && _planet.IsIncomeReady)
                _planet.GatherIncome();
        }

        private void OnPopupRequested()
        {
            if (_planet.IsUnlocked)
                _signalBus.Fire(new OnPlanetPopupRequestedSignal(_planet));
        }

        private void OnGathered(int range) =>
            _signalBus.Fire(new OnMoneyEarnedSignal(_view.CoinPosition, range));

        private void OnIncomeReady(bool isReady) =>
            _view.OnIncomeStatusChanged(isReady, _planet.IsUnlocked);

        private void OnUnlocked() => SetUnlocked(true);

        private void OnIncomeTimeChanged(float value)
        {
            string remainingTime = $"{Mathf.RoundToInt(value / 60)}m:{Mathf.RoundToInt(value % 60)}s";
            float progress = _planet.IncomeProgress;
            
            _view.SetIncomeProgress(remainingTime, progress);
        }

        private void SetUnlocked(bool isUnlocked)
        {
            var icon = _planet.GetIcon(isUnlocked);
            var price = _planet.Price.ToString();
            
            _view.SetIcon(icon);
            _view.SetUnlocked(isUnlocked);
            _view.SetPrice(price);
        }
    }
}
