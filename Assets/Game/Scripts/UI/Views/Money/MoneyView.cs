using Game.UI.Presentation.Money;
using R3;
using UnityEngine;
using Zenject;

namespace Game.UI.Views.Money
{
    public class MoneyView : MonoBehaviour
    {
        [SerializeField] private MoneyPanel _moneyPanel;
        
        private MoneyPresentation _presentation;

        [Inject]
        private void Construct(MoneyPresentation presentation) =>
            _presentation = presentation;

        private void Awake()
        {
            _presentation.OnMoneyEarned.Subscribe(OnMoneyEarned).AddTo(this);
            _presentation.OnMoneySpent.Subscribe(OnMoneySpent).AddTo(this);

            _moneyPanel.ChangeMoney(_presentation.Money);
        }

        private void OnMoneyEarned((Vector3 from, int cur, int prev) tuple) =>
            _moneyPanel.PlayGather(tuple.from, tuple.cur, tuple.prev);

        private void OnMoneySpent(int current) =>
            _moneyPanel.ChangeMoney(current);
    }
}
