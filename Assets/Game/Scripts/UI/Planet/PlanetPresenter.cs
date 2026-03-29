using System;
using Modules.Planets;
using UnityEngine;
using Zenject;

namespace Game.UI
{
    public class PlanetPresenter : MonoBehaviour, IInitializable, IDisposable
    {
        [SerializeField] private PlanetView _view;

        private IPlanet _planet;
        private MoneyPresenter _moneyPresenter;
        private PlanetPopupPresenter _planetPopupPresenter;

        public void Construct(
            IPlanet planet,
            MoneyPresenter moneyPresenter, 
            PlanetPopupPresenter planetPopupPresenter)
        {
            _planet = planet;
            _moneyPresenter = moneyPresenter;
            _planetPopupPresenter = planetPopupPresenter;
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
                _planetPopupPresenter.Show(_planet);
        }

        private void OnGathered(int range) =>
            _moneyPresenter.EarnMoney(_view.CoinPosition, range);

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
