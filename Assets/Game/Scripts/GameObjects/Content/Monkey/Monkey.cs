using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class Monkey :
        IInitializable,
        IFixedTickable,
        IDisposable, 
        LookComponent.ICondition
    {
        private readonly LookComponent _lookComponent;
        private readonly GroundedComponent _groundedComponent;
        private readonly TargetDetectorComponent _detectorComponent;
        private readonly HealthComponent _healthComponent;
        private readonly Rigidbody2D _rigidbody;
        private readonly ForceTargetComponent _jumpComponent;
        private readonly ForceAbilityComponent _pushComponent;
        private readonly CooldownComponent _jumpCooldownComponent;

        public Monkey(
            Rigidbody2D rigidbody,
            LookComponent lookComponent,
            GroundedComponent groundedComponent,
            TargetDetectorComponent detectorComponent,
            HealthComponent healthComponent,
            ForceTargetComponent jumpComponent,
            ForceAbilityComponent pushComponent,
            CooldownComponent jumpCooldownComponent)
        {
            _rigidbody = rigidbody;
            _jumpComponent = jumpComponent;
            _lookComponent = lookComponent;
            _groundedComponent = groundedComponent;
            _detectorComponent = detectorComponent;
            _healthComponent = healthComponent;
            _pushComponent = pushComponent;
            _jumpCooldownComponent = jumpCooldownComponent;
        }

        void IInitializable.Initialize()
        {
            _groundedComponent.OnGrounded += OnGroundedChanged;
            _detectorComponent.OnDetected += OnDetected;

            _lookComponent.SetCondition(this);
            _jumpComponent.SetCondition(() => _groundedComponent.IsGrounded);
        }

        void IDisposable.Dispose()
        {
            _groundedComponent.OnGrounded -= OnGroundedChanged;
            _detectorComponent.OnDetected -= OnDetected;
        }

        void IFixedTickable.FixedTick()
        {
            JumpTick();
        }

        private void JumpTick()
        {
            if(!_jumpCooldownComponent.IsExpired)
                return;
            
            _jumpComponent.ApplyForce(_rigidbody);
            _jumpCooldownComponent.Reset();
        }

        private void OnGroundedChanged(bool grounded)
        {
            if (grounded)
                _pushComponent.Apply();
        }

        private void OnDetected(Transform target) => _lookComponent.Look(target);

        bool LookComponent.ICondition.Evaluate() => _healthComponent.IsAlive;
    }
}
