using Game.Presentation;
using Modules.UI;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views
{
    public class PlanetView : MonoBehaviour
    {
        [SerializeField] private SmartButton _button;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _lock;
        [SerializeField] private Image _incomeProgressBar;
        [SerializeField] private TMP_Text _price;
        [SerializeField] private TMP_Text _incomeRemainingTime;
        [SerializeField] private GameObject _incomeProgressRoot;
        [SerializeField] private GameObject _priceRoot;
        [SerializeField] private GameObject _coin;

        private PlanetPresentation _presentation;

        public void Construct(PlanetPresentation presentation)
        {
            _presentation = presentation;
            _presentation.IsUnlocked.Subscribe(SetUnlocked).AddTo(this);
            _presentation.IncomeRemainingTime.Subscribe(v => _incomeRemainingTime.text = v).AddTo(this);
            _presentation.IncomeProgress.Subscribe(v => _incomeProgressBar.fillAmount = v).AddTo(this);

            Observable
                .CombineLatest(
                    _presentation.IsIncomeReady,
                    _presentation.IsUnlocked,
                    (isIncomeReady, isUnlocked) => (isIncomeReady, isUnlocked))
                .Subscribe(OnIncomeStatusChanged)
                .AddTo(this);
                
            Observable
                .FromEvent(h => _button.OnClick += h, h => _button.OnClick -= h)
                .Subscribe(_ => _presentation.OnPlanetClicked())
                .AddTo(this);

            Observable
                .FromEvent(h => _button.OnHold += h, h => _button.OnHold -= h)
                .Subscribe(_ => _presentation.OnPlanetPopupRequested())
                .AddTo(this);

            _presentation.OnCoinPositionSet(_coin.transform.position);
        }

        private void OnIncomeStatusChanged((bool isIncomeReady, bool isUnlocked) tuple)
        {
            bool isIncomeReady = tuple.isIncomeReady;
            bool isUnlocked = tuple.isUnlocked;
            
            _incomeProgressRoot.SetActive(!isIncomeReady && isUnlocked);
            _coin.SetActive(isIncomeReady && isUnlocked);
        }

        private void SetUnlocked(bool isUnlocked)
        {
            _lock.gameObject.SetActive(!isUnlocked);
            _incomeProgressRoot.SetActive(isUnlocked);
            _priceRoot.SetActive(!isUnlocked);
            _icon.sprite = _presentation.Sprite;
            _price.text = _presentation.Price;
        }
    }
}
