using System;
using Modules.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class PlanetView : MonoBehaviour
    {
        public event Action OnClick;
        public event Action OnHold;
        
        [SerializeField] private SmartButton _button;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _lock;
        [SerializeField] private Image _incomeProgressBar;
        [SerializeField] private TMP_Text _price;
        [SerializeField] private TMP_Text _incomeRemainingTime;
        [SerializeField] private GameObject _incomeProgressRoot;
        [SerializeField] private GameObject _priceRoot;
        [SerializeField] private GameObject _coin;

        public Vector3 CoinPosition => _coin.transform.position;
        
        private void OnEnable()
        {
            _button.OnClick += OnClickHandler;
            _button.OnHold += OnHoldHandler;
        }

        private void OnDisable()
        {
            _button.OnClick -= OnClickHandler;
            _button.OnHold -= OnHoldHandler;
        }

        public void SetIncomeProgress(string remainingTime, float progress01)
        {
            _incomeRemainingTime.text = remainingTime;
            _incomeProgressBar.fillAmount = progress01;
        }

        public void OnIncomeStatusChanged(bool isIncomeReady, bool isUnlocked)
        {
            _incomeProgressRoot.SetActive(!isIncomeReady && isUnlocked);
            _coin.SetActive(isIncomeReady && isUnlocked);
        }

        public void SetUnlocked(bool isUnlocked)
        {
            _lock.gameObject.SetActive(!isUnlocked);
            _incomeProgressRoot.SetActive(isUnlocked);
            _priceRoot.SetActive(!isUnlocked);
        }
        
        public void SetPrice(string price) => _price.text = price;
        
        public void SetIcon(Sprite sprite) => _icon.sprite = sprite;

        private void OnHoldHandler() => OnHold?.Invoke();

        private void OnClickHandler() => OnClick?.Invoke();
    }
}
