using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class DisableRigidbodyOnDeathComponent : IInitializable, IDisposable
    {
        private readonly HealthComponent _healthComponent;
        private readonly Rigidbody2D _rigidbody;

        public DisableRigidbodyOnDeathComponent(
            HealthComponent healthComponent,
            Rigidbody2D rigidbody)
        {
            _healthComponent = healthComponent;
            _rigidbody = rigidbody;
        }

        void IInitializable.Initialize() =>
            _healthComponent.OnDied += OnDied;

        void IDisposable.Dispose() =>
            _healthComponent.OnDied -= OnDied;

        private void OnDied() =>
            _rigidbody.simulated = false;
    }
}
