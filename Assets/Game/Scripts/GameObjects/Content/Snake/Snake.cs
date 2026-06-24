using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class Snake :
        IInitializable,
        IFixedTickable, 
        IDisposable,
        LookComponent.ICondition,
        MoveTransformComponent.ICondition
    {
        private readonly TargetComponent _targetComponent;
        private readonly LookComponent _lookComponent;
        private readonly ForceTargetComponent _pushComponent;
        private readonly CollisionComponent _collisionComponent;
        private readonly HealthComponent _healthComponent;
        private readonly MoveTransformComponent _moveComponent;
        private readonly GameEntity _gameEntity;

        private float _knockbackCooldownEnd;

        public Snake(
            TargetComponent targetComponent,
            LookComponent lookComponent,
            ForceTargetComponent pushComponent,
            CollisionComponent collisionComponent,
            HealthComponent healthComponent,
            MoveTransformComponent moveComponent,
            GameEntity gameEntity)
        {
            _targetComponent = targetComponent;
            _lookComponent = lookComponent;
            _pushComponent = pushComponent;
            _collisionComponent = collisionComponent;
            _healthComponent = healthComponent;
            _moveComponent = moveComponent;
            _gameEntity = gameEntity;
        }

        void IInitializable.Initialize()
        {
            _collisionComponent.OnEntered += OnCollisionEntered;
            _healthComponent.OnDied += OnDied;
            
            _lookComponent.SetCondition(this);
            _moveComponent.SetCondition(this);
        }

        void IDisposable.Dispose()
        {
            _collisionComponent.OnEntered -= OnCollisionEntered;
            _healthComponent.OnDied -= OnDied;
        }

        void IFixedTickable.FixedTick()
        {
            FollowTarget();
        }

        private void FollowTarget()
        {
            _lookComponent.Look(_targetComponent.Target);
            _moveComponent.Move(_targetComponent.Target, Time.fixedDeltaTime);
        }

        private void OnCollisionEntered(Collision2D collision)
        {
            _pushComponent.ApplyForce(collision.collider);
        }

        private void OnDied() =>
            UnityEngine.Object.Destroy(_gameEntity.gameObject);

        bool LookComponent.ICondition.Evaluate() => _healthComponent.IsAlive && _targetComponent.HasTarget;
        bool MoveTransformComponent.ICondition.Evaluate() => _healthComponent.IsAlive && _targetComponent.HasTarget;
    }
}
