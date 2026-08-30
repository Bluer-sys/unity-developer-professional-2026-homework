using System;
using Game.Money;
using TMPro;
using UnityEngine;

namespace Game
{
    public class MoneyPresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _moneyText;
        [SerializeField] private MoneyStorage _moneyStorage;

        private void OnEnable()
        {
            _moneyStorage.OnStateChanged += UpdateMoney;
        }

        private void OnDisable()
        {
            _moneyStorage.OnStateChanged -= UpdateMoney;
        }

        private void UpdateMoney(int value)
        {
            _moneyText.text = value.ToString();
        }
    }
}
