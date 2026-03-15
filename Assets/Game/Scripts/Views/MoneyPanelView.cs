using Game.Presentation;
using R3;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Views
{
    public class MoneyPanelView : MonoBehaviour
    {
        [SerializeField] private RectTransform _iconTransform;
        [SerializeField] private TMP_Text _money;

        public Vector3 CoinPosition => _iconTransform.position;
        
        private MoneyPanelPresentation _presentation;

        [Inject]
        private void Construct(MoneyPanelPresentation presentation) =>
            _presentation = presentation;

        private void Awake()
        {
            _presentation.Money.Subscribe(v => _money.text = v).AddTo(this);
        }
    }
}
