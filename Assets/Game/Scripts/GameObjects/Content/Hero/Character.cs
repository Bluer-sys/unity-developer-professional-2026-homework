using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class Character :
        IInitializable,
        IDisposable,
        MoveTransformComponent.ICondition,
        LookComponent.ICondition
    {
        private readonly HealthComponent _healthComponent;
        private readonly MoveTransformComponent _moveComponent;
        private readonly LookComponent _lookComponent;
        private readonly GroundedComponent _groundedComponent;
        private readonly JumpComponent _jumpComponent;
        private readonly Rigidbody2D _rigidbody;
        private readonly PushAbilityComponent _pushComponent;
        private readonly BlowUpAbilityComponent _blowUpComponent;

        public Character(
                HealthComponent healthComponent,
                MoveTransformComponent moveComponent,
                LookComponent lookComponent,
                GroundedComponent groundedComponent,
                JumpComponent jumpComponent,
                Rigidbody2D rigidbody,
                PushAbilityComponent pushComponent,
                BlowUpAbilityComponent blowUpComponent
            )
        {
            _healthComponent = healthComponent;
            _moveComponent = moveComponent;
            _lookComponent = lookComponent;
            _groundedComponent = groundedComponent;
            _jumpComponent = jumpComponent;
            _rigidbody = rigidbody;
            _pushComponent = pushComponent;
            _blowUpComponent = blowUpComponent;
        }

        void IInitializable.Initialize()
        {
            _moveComponent.SetCondition(this);
            _lookComponent.SetCondition(this);
            
            _jumpComponent.SetCondition(() => _healthComponent.IsAlive && _groundedComponent.IsGrounded);
            _pushComponent.SetCondition(() => _healthComponent.IsAlive && !_blowUpComponent.IsProcessing);
            _blowUpComponent.SetCondition(() => _healthComponent.IsAlive && !_pushComponent.IsProcessing && _groundedComponent.IsGrounded);
            
            _healthComponent.OnDied += OnDied;
        }

        public void Dispose()
        {
            _healthComponent.OnDied -= OnDied;
        }

        bool MoveTransformComponent.ICondition.Evaluate() => _healthComponent.IsAlive;
        bool LookComponent.ICondition.Evaluate() => _healthComponent.IsAlive;

        private void OnDied() =>
            _rigidbody.simulated = false;
    }
}
