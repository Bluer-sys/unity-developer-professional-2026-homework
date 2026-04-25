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
        private readonly KnockbackComponent _knockbackComponent;

        public Trampoline(
            Settings settings,
            TriggerComponent triggerComponent,
            KnockbackComponent knockbackComponent)
        {
            _settings = settings;
            _triggerComponent = triggerComponent;
            _knockbackComponent = knockbackComponent;
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
            _knockbackComponent.TryKnockback(other, _settings.Force);
        }
    }
}
