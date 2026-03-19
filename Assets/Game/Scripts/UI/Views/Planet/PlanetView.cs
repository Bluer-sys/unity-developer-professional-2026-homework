using Game.UI.Presentation.Planet;
using Modules.UI;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.UI.Views.Planet
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

        [Inject]
        private void Construct(PlanetPresentation presentation) =>
            _presentation = presentation;
        
        public void Awake()
        {
            _presentation.IsUnlocked.Subscribe(SetUnlocked).AddTo(this);
            _presentation.IncomeRemainingTime.Subscribe(v => _incomeRemainingTime.text = v).AddTo(this);
            _presentation.IncomeProgress.Subscribe(v => _incomeProgressBar.fillAmount = v).AddTo(this);
            _presentation.IsIncomeReady.Select(v => !v).Subscribe(_incomeProgressRoot.SetActive).AddTo(this);

            Observable
                .CombineLatest(
                    _presentation.IsIncomeReady,
                    _presentation.IsUnlocked,
                    (isIncomeReady, isUnlocked) => isIncomeReady && isUnlocked)
                .Subscribe(_coin.SetActive)
                .AddTo(this);
                
            _button.OnClick += OnButtonClick;
            _button.OnHold += OnButtonHold;
            
            _incomeProgressRoot.SetActive(false);
            
            _presentation.OnCoinPositionSet(_coin.transform.position);
            _presentation.Initialize();
        }

        private void OnDestroy()
        {
            _button.OnClick -= OnButtonClick;
            _button.OnHold -= OnButtonHold;
            
            _presentation.Dispose();
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
