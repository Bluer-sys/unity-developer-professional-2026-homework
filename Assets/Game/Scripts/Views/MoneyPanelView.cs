using DG.Tweening;
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
        
        [Header("Settings")]
        [SerializeField] private float _changeAnimDuration;

        public Vector3 CoinPosition => _iconTransform.position;
        
        private MoneyPanelPresentation _presentation;
        private Tween _moneyChangeTween;

        [Inject]
        private void Construct(MoneyPanelPresentation presentation) =>
            _presentation = presentation;

        private void Awake()
        {
            _presentation.Money
                         .Pairwise()
                         .Where(pair => pair.Current > pair.Previous)
                         .Subscribe(PlayChangeMoney)
                         .AddTo(this);
            
            _presentation.Money
                         .Pairwise()
                         .Where(pair => pair.Current < pair.Previous)
                         .Select(pair => pair.Current)
                         .Subscribe(ChangeMoney)
                         .AddTo(this);
        }

        private void PlayChangeMoney((int Previous, int Current) pair)
        {
            _moneyChangeTween?.Kill(true);
            _moneyChangeTween = DOVirtual
                                .Int(pair.Previous, pair.Current, _changeAnimDuration, ChangeMoney)
                                .OnComplete(() =>
                                {
                                    ChangeMoney(pair.Current);
                                    _moneyChangeTween = null;
                                });
        }

        private void ChangeMoney(int current) =>
            _money.text = _presentation.Formate(current);
    }
}
