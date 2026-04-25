using System;
using Zenject;

namespace Game
{
    public sealed class Trap : IInitializable, IDisposable
    {
        private readonly HealthComponent _healthComponent;
        private readonly DamageOnContactComponent _damageOnContact;

        public Trap(
            HealthComponent healthComponent,
            DamageOnContactComponent damageOnContact)
        {
            _healthComponent = healthComponent;
            _damageOnContact = damageOnContact;
        }

        void IInitializable.Initialize() =>
            _damageOnContact.OnDamageDealt += OnDamageDealt;

        void IDisposable.Dispose() =>
            _damageOnContact.OnDamageDealt -= OnDamageDealt;

        private void OnDamageDealt(HealthComponent _) =>
            _healthComponent.SetZero();
    }
}
