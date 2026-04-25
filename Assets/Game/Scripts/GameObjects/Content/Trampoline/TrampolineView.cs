using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;
using Zenject;

namespace Game
{
    public sealed class TrampolineView : MonoBehaviour
    {
        private static readonly int ThrowUp = Animator.StringToHash(nameof(ThrowUp));

        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private Animator _animator;

        private TriggerComponent _triggerComponent;
        private Tween _tween;

        [Inject]
        private void Construct(TriggerComponent triggerComponent)
        {
            _triggerComponent = triggerComponent;
        }

        private void OnEnable() => _triggerComponent.OnEntered += OnEntered;

        private void OnDisable()
        {
            _triggerComponent.OnEntered -= OnEntered;
            _tween?.Kill();
        }

        [Button]
        private void OnEntered(Collider2D _)
        {
            _audioSource.Play();

            _tween?.Kill();

            _tween = DOTween.Sequence()
                .Append(DOTween.To(
                    () => _animator.GetFloat(ThrowUp),
                    x => _animator.SetFloat(ThrowUp, x),
                    1f,
                    0.1f))
                .Append(DOTween.To(
                    () => _animator.GetFloat(ThrowUp),
                    x => _animator.SetFloat(ThrowUp, x),
                    0f,
                    0.15f))
                .Append(DOTween.To(
                    () => _animator.GetFloat(ThrowUp),
                    x => _animator.SetFloat(ThrowUp, x),
                    0.5f,
                    0.2f))
                .SetEase(Ease.OutQuad);
        }
    }
}
