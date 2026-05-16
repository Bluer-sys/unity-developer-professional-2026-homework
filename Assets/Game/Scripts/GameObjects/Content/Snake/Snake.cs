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
        MoveComponent.ICondition
    {
        [Serializable]
        public class Settings
        {
            [field: SerializeField]
            public Vector2 KnockbackForce { get; private set; }
        }

        private readonly Settings _settings;
        private readonly TargetDetectorComponent _detectorComponent;
        private readonly LookComponent _lookComponent;
        private readonly PushComponent _pushComponent;
        private readonly CollisionComponent _collisionComponent;
        private readonly HealthComponent _healthComponent;
        private readonly MoveComponent _moveComponent;

        private float _knockbackCooldownEnd;

        public Snake(
            Settings settings,
            TargetDetectorComponent detectorComponent,
            LookComponent lookComponent,
            PushComponent pushComponent,
            CollisionComponent collisionComponent,
            HealthComponent healthComponent,
            MoveComponent moveComponent)
        {
            _settings = settings;
            _detectorComponent = detectorComponent;
            _lookComponent = lookComponent;
            _pushComponent = pushComponent;
            _collisionComponent = collisionComponent;
            _healthComponent = healthComponent;
            _moveComponent = moveComponent;
        }

        void IInitializable.Initialize()
        {
            _collisionComponent.OnEntered += OnCollisionEntered;
            
            _lookComponent.SetCondition(this);
            _moveComponent.SetCondition(this);
        }

        void IDisposable.Dispose() => _collisionComponent.OnEntered -= OnCollisionEntered;
        
        void IFixedTickable.FixedTick() => FollowTarget();

        private void FollowTarget()
        {
            if (!_detectorComponent.HasTarget)
                return;

            _lookComponent.Look(_detectorComponent.Target);
            _moveComponent.Move(_detectorComponent.Target, Time.fixedDeltaTime);
        }

        private void OnCollisionEntered(Collision2D collision)
        {
            _pushComponent.TryPush(collision.collider, _settings.KnockbackForce);
        }

        bool LookComponent.ICondition.Evaluate() => _healthComponent.IsAlive;
        bool MoveComponent.ICondition.Evaluate() => _healthComponent.IsAlive;
    }
}
