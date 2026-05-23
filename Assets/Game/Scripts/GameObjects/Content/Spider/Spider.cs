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

        private float _knockbackCooldownEnd;

        public Spider(
            LookComponent lookComponent,
            PatrolComponent patrolComponent,
            ForceTargetComponent pushComponent,
            CollisionComponent collisionComponent,
            HealthComponent healthComponent,
            MoveTransformComponent moveComponent)
        {
            _lookComponent = lookComponent;
            _patrolComponent = patrolComponent;
            _pushComponent = pushComponent;
            _collisionComponent = collisionComponent;
            _healthComponent = healthComponent;
            _moveComponent = moveComponent;
        }

        void IInitializable.Initialize()
        {
            _collisionComponent.OnEntered += OnCollisionEntered;
            _healthComponent.OnDied += OnDied;
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
        }
    }
}
