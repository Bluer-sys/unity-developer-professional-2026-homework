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
        [Serializable]
        public class Settings
        {
            [field: SerializeField]
            public Vector2 KnockbackForce { get; private set; }

            [field: SerializeField]
            public float KnockbackCooldown { get; private set; }

            [field: SerializeField]
            public float JumpInterval { get; private set; }

            [field: SerializeField]
            public float WaveRadius { get; private set; }

            [field: SerializeField]
            public LayerMask WaveMask { get; private set; }
        }

        private readonly Settings _settings;
        private readonly TransformComponent _transformComponent;
        private readonly LookComponent _lookComponent;
        private readonly GroundedComponent _groundedComponent;
        private readonly JumpComponent _jumpComponent;
        private readonly TargetDetectorComponent _detectorComponent;
        private readonly PushComponent _pushComponent;
        private readonly CollisionComponent _collisionComponent;
        private readonly HealthComponent _healthComponent;

        private float _jumpTimer;
        private bool _waitingForLanding;
        private float _knockbackCooldownEnd;

        public Monkey(
            Settings settings,
            TransformComponent transformComponent,
            LookComponent lookComponent,
            GroundedComponent groundedComponent,
            JumpComponent jumpComponent,
            TargetDetectorComponent detectorComponent,
            PushComponent pushComponent,
            CollisionComponent collisionComponent,
            HealthComponent healthComponent)
        {
            _settings = settings;
            _transformComponent = transformComponent;
            _lookComponent = lookComponent;
            _groundedComponent = groundedComponent;
            _jumpComponent = jumpComponent;
            _detectorComponent = detectorComponent;
            _pushComponent = pushComponent;
            _collisionComponent = collisionComponent;
            _healthComponent = healthComponent;
        }

        void IInitializable.Initialize()
        {
            _groundedComponent.OnGrounded += OnGroundedChanged;
            _jumpComponent.OnJumped += OnJumped;
            _detectorComponent.OnDetected += OnDetected;
            _collisionComponent.OnEntered += OnCollisionEntered;
            _healthComponent.OnDied += OnDied;

            _lookComponent.SetCondition(this);
            
            _jumpTimer = _settings.JumpInterval;
        }

        void IDisposable.Dispose()
        {
            _groundedComponent.OnGrounded -= OnGroundedChanged;
            _jumpComponent.OnJumped -= OnJumped;
            _detectorComponent.OnDetected -= OnDetected;
            _collisionComponent.OnEntered -= OnCollisionEntered;
            _healthComponent.OnDied -= OnDied;
        }

        void IFixedTickable.FixedTick()
        {
            if (!_healthComponent.IsAlive)
                return;

            if (_detectorComponent.HasTarget)
                _lookComponent.Look(_detectorComponent.Target);

            if (_jumpTimer > 0)
            {
                _jumpTimer -= Time.fixedDeltaTime;
                return;
            }

            if (_groundedComponent.IsGrounded)
            {
                _jumpComponent.Jump();
                _jumpTimer = _settings.JumpInterval;
            }
        }

        private void OnJumped() =>
            _waitingForLanding = true;

        private void OnGroundedChanged(bool grounded)
        {
            if (!grounded)
                return;

            if (!_waitingForLanding)
                return;

            _waitingForLanding = false;
            SpawnWave();
        }

        private void SpawnWave()
        {
            Vector2 center = _transformComponent.Transform.position;
            Collider2D[] hits = Physics2D.OverlapCircleAll(center, _settings.WaveRadius, _settings.WaveMask);

            foreach (Collider2D hit in hits)
                _pushComponent.TryPush(hit, _settings.KnockbackForce);
        }

        private void OnDetected(Transform target) =>
            _lookComponent.Look(target);

        private void OnCollisionEntered(Collision2D collision)
        {
            if (Time.time < _knockbackCooldownEnd)
                return;

            if (_pushComponent.TryPush(collision.collider, _settings.KnockbackForce))
                _knockbackCooldownEnd = Time.time + _settings.KnockbackCooldown;
        }

        private void OnDied() =>
            _waitingForLanding = false;

        
        bool LookComponent.ICondition.Evaluate() => _healthComponent.IsAlive;
    }
}
