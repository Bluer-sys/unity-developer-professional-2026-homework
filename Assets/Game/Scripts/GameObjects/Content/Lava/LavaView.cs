using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class LavaView : IInitializable, IDisposable
    {
        private readonly AudioSource _audioSource;
        private readonly TriggerComponent _triggerComponent;

        public LavaView(TriggerComponent triggerComponent, AudioSource audioSource)
        {
            _triggerComponent = triggerComponent;
            _audioSource = audioSource;
        }

        public void Initialize() =>
            _triggerComponent.OnEntered += OnEntered;

        public void Dispose() =>
            _triggerComponent.OnEntered -= OnEntered;

        private void OnEntered(Collider2D collider) 
            => _audioSource.Play();
    }
}
