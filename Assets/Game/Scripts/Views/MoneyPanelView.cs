using Game.Presentation;
using R3;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Views
{
    public class MoneyPanelView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _money;

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
