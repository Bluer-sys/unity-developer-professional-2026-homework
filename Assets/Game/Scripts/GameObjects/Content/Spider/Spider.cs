using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class Spider : IInitializable, IDisposable
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
        private readonly LookComponent _lookComponent;
        private readonly PatrolComponent _patrolComponent;
        private readonly KnockbackComponent _knockbackComponent;
        private readonly CollisionComponent _collisionComponent;
        private readonly HealthComponent _healthComponent;

        private float _knockbackCooldownEnd;

        public Spider(
            Settings settings,
            LookComponent lookComponent,
            PatrolComponent patrolComponent,
            KnockbackComponent knockbackComponent,
            CollisionComponent collisionComponent,
            HealthComponent healthComponent)
        {
            _settings = settings;
            _lookComponent = lookComponent;
            _patrolComponent = patrolComponent;
            _knockbackComponent = knockbackComponent;
            _collisionComponent = collisionComponent;
            _healthComponent = healthComponent;
        }

        void IInitializable.Initialize()
        {
            _patrolComponent.OnDirectionChanged += OnDirectionChanged;
            _collisionComponent.OnEntered += OnCollisionEntered;
            _healthComponent.OnDied += OnDied;

            _lookComponent.Look(_patrolComponent.Direction);
        }

        void IDisposable.Dispose()
        {
            _patrolComponent.OnDirectionChanged -= OnDirectionChanged;
            _collisionComponent.OnEntered -= OnCollisionEntered;
            _healthComponent.OnDied -= OnDied;
        }

        private void OnDirectionChanged(int direction) =>
            _lookComponent.Look(direction);

        private void OnCollisionEntered(Collision2D collision)
        {
            if (Time.time < _knockbackCooldownEnd)
                return;

            if (_knockbackComponent.TryKnockback(collision.collider, _settings.KnockbackForce))
                _knockbackCooldownEnd = Time.time + _settings.KnockbackCooldown;
        }

        private void OnDied() =>
            _patrolComponent.Disable();
    }
}
