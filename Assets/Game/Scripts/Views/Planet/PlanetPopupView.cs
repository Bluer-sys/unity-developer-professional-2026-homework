using Game.Presentation;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Views
{
    public class PlanetPopupView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _label;
        [SerializeField] private TMP_Text _population;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private TMP_Text _income;
        [SerializeField] private TMP_Text _upgradeLabel;
        [SerializeField] private TMP_Text _upgradePrice;
        [SerializeField] private GameObject _upgradePriceRoot;
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Image _preview;

        private PlanetPopupPresentation _presentation;

        [Inject]
        private void Construct(PlanetPopupPresentation presentation)
        {
            _presentation = presentation;

            _presentation.Label.Subscribe(v => _label.text = v).AddTo(this);
            _presentation.Population.Subscribe(v => _population.text = v).AddTo(this);
            _presentation.Level.Subscribe(v => _level.text = v).AddTo(this);
            _presentation.Income.Subscribe(v => _income.text = v).AddTo(this);
            _presentation.UpgradeLabel.Subscribe(v => _upgradeLabel.text = v).AddTo(this);
            _presentation.UpgradePrice.Subscribe(v => _upgradePrice.text = v).AddTo(this);
            _presentation.IsVisible.Subscribe(v => gameObject.SetActive(v)).AddTo(this);
            _presentation.IsUpgradeButtonInteractable.Subscribe(v => _upgradeButton.interactable = v).AddTo(this);
            _presentation.IsPriceVisible.Subscribe(v => _upgradePriceRoot.SetActive(v)).AddTo(this);
            _presentation.Sprite.Subscribe(v => _preview.sprite = v).AddTo(this);

            _upgradeButton.OnClickAsObservable().Subscribe(_ => _presentation.Upgrade()).AddTo(this);
            _closeButton.OnClickAsObservable().Subscribe(_ => _presentation.Hide()).AddTo(this);
        }
    }
}
