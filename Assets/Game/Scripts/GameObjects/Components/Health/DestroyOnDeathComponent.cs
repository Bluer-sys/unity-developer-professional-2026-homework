using System;
using Zenject;

namespace Game
{
    public sealed class DestroyOnDeathComponent : IInitializable, IDisposable
    {
        private readonly HealthComponent _healthComponent;
        private readonly GameEntity _gameEntity;

        public DestroyOnDeathComponent(
            HealthComponent healthComponent,
            GameEntity gameEntity)
        {
            _healthComponent = healthComponent;
            _gameEntity = gameEntity;
        }

        void IInitializable.Initialize() =>
            _healthComponent.OnDied += OnDied;

        void IDisposable.Dispose() =>
            _healthComponent.OnDied -= OnDied;

        private void OnDied() =>
            UnityEngine.Object.Destroy(_gameEntity.gameObject);
    }
}
