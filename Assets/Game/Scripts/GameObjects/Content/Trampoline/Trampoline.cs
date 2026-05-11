using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class Trampoline : IInitializable, IDisposable
    {
        [Serializable]
        public class Settings
        {
            [field: SerializeField]
            public Vector2 Force { get; private set; }
        }

        private readonly Settings _settings;
        private readonly TriggerComponent _triggerComponent;
        private readonly PushComponent _pushComponent;

        public Trampoline(
            Settings settings,
            TriggerComponent triggerComponent,
            PushComponent pushComponent)
        {
            _settings = settings;
            _triggerComponent = triggerComponent;
            _pushComponent = pushComponent;
        }

        void IInitializable.Initialize() =>
            _triggerComponent.OnEntered += OnEntered;

        void IDisposable.Dispose() =>
            _triggerComponent.OnEntered -= OnEntered;

        private void OnEntered(Collider2D other)
        {
            Rigidbody2D rb = other.attachedRigidbody;
            if (rb == null)
                return;

            rb.linearVelocityY = 0;
            _pushComponent.TryPush(other, _settings.Force);
        }
    }
}
