using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class Spider : IInitializable, IDisposable, IFixedTickable
    {
        private readonly LookComponent _lookComponent;
        private readonly PatrolComponent _patrolComponent;
        private readonly ForceTargetComponent _pushComponent;
        private readonly CollisionComponent _collisionComponent;
        private readonly HealthComponent _healthComponent;
        private readonly MoveTransformComponent _moveComponent;
        private readonly GroundedComponent _groundedComponent;
        private readonly Rigidbody2D _rigidbody;

        private float _knockbackCooldownEnd;

        public Spider(
            LookComponent lookComponent,
            PatrolComponent patrolComponent,
            ForceTargetComponent pushComponent,
            CollisionComponent collisionComponent,
            HealthComponent healthComponent,
            MoveTransformComponent moveComponent,
            GroundedComponent groundedComponent,
            Rigidbody2D rigidbody)
        {
            _lookComponent = lookComponent;
            _patrolComponent = patrolComponent;
            _pushComponent = pushComponent;
            _collisionComponent = collisionComponent;
            _healthComponent = healthComponent;
            _moveComponent = moveComponent;
            _groundedComponent = groundedComponent;
            _rigidbody = rigidbody;
        }

        void IInitializable.Initialize()
        {
            _collisionComponent.OnEntered += OnCollisionEntered;
            _healthComponent.OnDied += OnDied;

            _pushComponent.SetCondition(() => _healthComponent.IsAlive && _groundedComponent.IsGrounded);
        }

        void IDisposable.Dispose()
        {
            _collisionComponent.OnEntered -= OnCollisionEntered;
            _healthComponent.OnDied -= OnDied;
        }

        void IFixedTickable.FixedTick()
        {
            _lookComponent.Look(_moveComponent.MoveDirection.x);
        }

        private void OnCollisionEntered(Collision2D collision)
        {
            _pushComponent.ApplyForce(collision.collider);
        }

        private void OnDied()
        {
            _patrolComponent.Disable();
            _rigidbody.simulated = false;
        }
    }
}
