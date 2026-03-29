using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
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

        public Button.ButtonClickedEvent OnUpgrade => _upgradeButton.onClick;
        public Button.ButtonClickedEvent OnClose => _closeButton.onClick;
        
        public void SetVisible(bool state) => gameObject.SetActive(state);
        public void SetPriceVisible(bool state) => _upgradePriceRoot.SetActive(state);
        public void SetLabelText(string value) => _label.text = value;
        public void SetPopulation(string value) => _population.text = value;
        public void SetLevel(string value) => _level.text = value;
        public void SetIncome(string value) => _income.text = value;
        public void SetUpgradeLabel(string value) => _upgradeLabel.text = value;
        public void SetUpgradePrice(string value) => _upgradePrice.text = value;
        public void SetUpgradeButtonInteractable(bool value) => _upgradeButton.interactable = value;
        public void SetSprite(Sprite sprite) => _preview.sprite = sprite;
    }
}
