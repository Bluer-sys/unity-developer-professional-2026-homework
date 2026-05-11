using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class Snake : IInitializable, IFixedTickable, IDisposable
    {
        [Serializable]
        public class Settings
        {
            [field: SerializeField]
            public Vector2 KnockbackForce { get; private set; }

            [field: SerializeField]
            public float KnockbackCooldown { get; private set; }
        }

        private readonly Settings _settings;
        private readonly TargetDetectorComponent _detectorComponent;
        private readonly ChaseComponent _chaseComponent;
        private readonly LookComponent _lookComponent;
        private readonly PushComponent _pushComponent;
        private readonly CollisionComponent _collisionComponent;
        private readonly HealthComponent _healthComponent;

        private float _knockbackCooldownEnd;

        public Snake(
            Settings settings,
            TargetDetectorComponent detectorComponent,
            ChaseComponent chaseComponent,
            LookComponent lookComponent,
            PushComponent pushComponent,
            CollisionComponent collisionComponent,
            HealthComponent healthComponent)
        {
            _settings = settings;
            _detectorComponent = detectorComponent;
            _chaseComponent = chaseComponent;
            _lookComponent = lookComponent;
            _pushComponent = pushComponent;
            _collisionComponent = collisionComponent;
            _healthComponent = healthComponent;
        }

        void IInitializable.Initialize()
        {
            _detectorComponent.OnDetected += OnDetected;
            _detectorComponent.OnLost += OnLost;
            _collisionComponent.OnEntered += OnCollisionEntered;
            _healthComponent.OnDied += OnDied;
        }

        void IDisposable.Dispose()
        {
            _detectorComponent.OnDetected -= OnDetected;
            _detectorComponent.OnLost -= OnLost;
            _collisionComponent.OnEntered -= OnCollisionEntered;
            _healthComponent.OnDied -= OnDied;
        }

        void IFixedTickable.FixedTick()
        {
            if (!_healthComponent.IsAlive)
                return;

            if (_detectorComponent.HasTarget)
                _lookComponent.Look(_detectorComponent.Target);
        }

        private void OnDetected(Transform target)
        {
            _chaseComponent.SetTarget(target);
            _chaseComponent.Enable();
            _lookComponent.Look(target);
        }

        private void OnLost()
        {
            _chaseComponent.Disable();
            _chaseComponent.SetTarget(null);
        }

        private void OnCollisionEntered(Collision2D collision)
        {
            if (Time.time < _knockbackCooldownEnd)
                return;

            if (_pushComponent.TryPush(collision.collider, _settings.KnockbackForce))
                _knockbackCooldownEnd = Time.time + _settings.KnockbackCooldown;
        }

        private void OnDied()
        {
            _chaseComponent.Disable();
            _chaseComponent.SetTarget(null);
        }
    }
}
