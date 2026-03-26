using DG.Tweening;
using Modules.UI;
using TMPro;
using UnityEngine;

namespace Game.Views
{
    public class MoneyPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _money;
        [SerializeField] private RectTransform _iconTransform;
        [SerializeField] private ParticleAnimator _particleAnimator;

        [Header("Settings")]
        [SerializeField] private float _particleDuration;
        [SerializeField] private float _changeMoneyDuration;

        private Tween _moneyChangeTween;

        public void PlayGather(Vector3 from, int current, int previous)
        {
            _particleAnimator.Emit(from, _iconTransform.position, _particleDuration,
                () => PlayChangeMoney(current, previous));
        }

        public void PlayChangeMoney(int current, int previous)
        {
            Kill();
            _moneyChangeTween = DOVirtual
                                .Int(previous, current, _changeMoneyDuration, SetMoney)
                                .OnComplete(() =>
                                {
                                    ChangeMoney(current);
                                    _moneyChangeTween = null;
                                });
        }

        public void ChangeMoney(int current)
        {
            Kill();
            SetMoney(current);
        }

        private void SetMoney(int value) => _money.text = value.ToString();
        private void Kill() => _moneyChangeTween?.Kill(true);
    }
}
