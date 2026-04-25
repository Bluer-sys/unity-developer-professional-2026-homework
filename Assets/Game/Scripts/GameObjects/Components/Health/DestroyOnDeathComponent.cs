using System;
using Zenject;

namespace Game
{
    public sealed class DestroyOnDeathComponent : IInitializable, IDisposable
    {
        private readonly HealthComponent _healthComponent;
        private readonly TransformComponent _transformComponent;

        public DestroyOnDeathComponent(
            HealthComponent healthComponent,
            TransformComponent transformComponent)
        {
            _healthComponent = healthComponent;
            _transformComponent = transformComponent;
        }

        void IInitializable.Initialize() =>
            _healthComponent.OnDied += OnDied;

        void IDisposable.Dispose() =>
            _healthComponent.OnDied -= OnDied;

        private void OnDied() =>
            UnityEngine.Object.Destroy(_transformComponent.Transform.gameObject);
    }
}
