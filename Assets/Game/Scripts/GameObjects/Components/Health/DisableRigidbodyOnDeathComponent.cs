using System;
using Zenject;

namespace Game
{
    public sealed class DisableRigidbodyOnDeathComponent : IInitializable, IDisposable
    {
        private readonly HealthComponent _healthComponent;
        private readonly RigidbodyComponent _rigidbodyComponent;

        public DisableRigidbodyOnDeathComponent(
            HealthComponent healthComponent,
            RigidbodyComponent rigidbodyComponent)
        {
            _healthComponent = healthComponent;
            _rigidbodyComponent = rigidbodyComponent;
        }

        void IInitializable.Initialize() =>
            _healthComponent.OnDied += OnDied;

        void IDisposable.Dispose() =>
            _healthComponent.OnDied -= OnDied;

        private void OnDied() =>
            _rigidbodyComponent.Rigidbody.simulated = false;
    }
}
