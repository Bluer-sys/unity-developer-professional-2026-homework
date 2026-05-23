using System;
using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;
using Zenject;

namespace Game
{
    public sealed class TrampolineView : IInitializable, IDisposable
    {
        private static readonly int _throwUp = Animator.StringToHash("ThrowUp");

        private readonly AudioSource _audioSource;
        private readonly Animator _animator;
        private readonly TriggerComponent _triggerComponent;
        
        private Tween _tween;

        public TrampolineView(
            Animator animator, 
            AudioSource audioSource,
            TriggerComponent triggerComponent)
        {
            _animator = animator;
            _audioSource = audioSource;
            _triggerComponent = triggerComponent;
        }

        public void Initialize()
        {
            _triggerComponent.OnEntered += OnEntered;
        }

        public void Dispose()
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
                                () => _animator.GetFloat(_throwUp),
                                x => _animator.SetFloat(_throwUp, x),
                                1f,
                                0.1f))
                            .Append(DOTween.To(
                                () => _animator.GetFloat(_throwUp),
                                x => _animator.SetFloat(_throwUp, x),
                                0f,
                                0.15f))
                            .Append(DOTween.To(
                                () => _animator.GetFloat(_throwUp),
                                x => _animator.SetFloat(_throwUp, x),
                                0.5f,
                                0.2f))
                            .SetEase(Ease.OutQuad);
        }
    }
}
