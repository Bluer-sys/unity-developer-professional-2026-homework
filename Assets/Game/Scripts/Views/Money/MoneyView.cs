using UnityEngine;

namespace Game.Views
{
    public class MoneyView : MonoBehaviour
    {
        [SerializeField] private MoneyPanel _moneyPanel;
        
        public void ChangeMoney(int current) =>
            _moneyPanel.ChangeMoney(current);

        public void PlayEarnMoney(Vector3 from, int cur, int prev) =>
            _moneyPanel.PlayGather(from, cur, prev);
    }
}
