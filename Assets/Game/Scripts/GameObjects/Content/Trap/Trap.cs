using System;
using Zenject;

namespace Game
{
    public sealed class Trap : IInitializable, IDisposable
    {
        private readonly HealthComponent _healthComponent;
        private readonly DamageOnContactComponent _damageOnContact;
        private readonly GameEntity _gameEntity;

        public Trap(
            HealthComponent healthComponent,
            DamageOnContactComponent damageOnContact,
            GameEntity gameEntity)
        {
            _healthComponent = healthComponent;
            _damageOnContact = damageOnContact;
            _gameEntity = gameEntity;
        }

        void IInitializable.Initialize()
        {
            _damageOnContact.OnDamageDealt += OnDamageDealt;
            _healthComponent.OnDied += OnDied;
        }

        void IDisposable.Dispose()
        {
            _damageOnContact.OnDamageDealt -= OnDamageDealt;
            _healthComponent.OnDied -= OnDied;
        }

        private void OnDamageDealt(HealthComponent _) =>
            _healthComponent.SetZero();

        private void OnDied() =>
            UnityEngine.Object.Destroy(_gameEntity.gameObject);
    }
}
