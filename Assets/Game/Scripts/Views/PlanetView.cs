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
        [SerializeField] private ParticleAnimator _particleAnimator;

        public Vector3 CoinPosition => _coin.transform.position;
        
        private PlanetPresentation _presentation;

        public void Initialize(PlanetPresentation presentation)
        {
            _presentation = presentation;
            _presentation.IsUnlocked.Subscribe(SetUnlocked).AddTo(this);
            _presentation.IncomeRemainingTime.Subscribe(v => _incomeRemainingTime.text = v).AddTo(this);
            _presentation.IncomeProgress.Subscribe(v => _incomeProgressBar.fillAmount = v).AddTo(this);
            _presentation.IsIncomeReady.Select(v => !v).Subscribe(_incomeProgressRoot.SetActive).AddTo(this);

            Observable
                .CombineLatest(
                    _presentation.IsGatherProcess,
                    _presentation.IsIncomeReady,
                    _presentation.IsUnlocked,
                    (isGatherProcess, isIncomeReady, isUnlocked) => !isGatherProcess && isIncomeReady && isUnlocked)
                .Subscribe(_coin.SetActive)
                .AddTo(this);
                
            _button.OnClick += OnButtonClick;
            _button.OnHold += OnButtonHold;
            
            _incomeProgressRoot.SetActive(false);
        }

        private void OnDestroy()
        {
            _button.OnClick -= OnButtonClick;
            _button.OnHold -= OnButtonHold;
        }

        private void OnButtonClick() =>
            _presentation.OnPlanetClicked();

        private void OnButtonHold() =>
            _presentation.OnPlanetPopupRequested();

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
