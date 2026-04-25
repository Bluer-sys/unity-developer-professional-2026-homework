using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class Character :
        IInitializable,
        ITickable,
        IFixedTickable,
        IDisposable,
        MoveComponent.ICondition,
        JumpComponent.ICondition
    {
        [Serializable]
        public class Settings
        {
            [Serializable]
            public class Ability
            {
                [field: SerializeField]
                public Vector2 Force { get; private set; }

                [field: SerializeField]
                public float Cooldown { get; private set; }

                [field: SerializeField]
                public float Delay { get; private set; }

                [field: SerializeField]
                public Vector2 OverlapSize { get; private set; }

                [field: SerializeField]
                public Vector2 OverlapOffset { get; private set; }

                [field: SerializeField]
                public LayerMask Mask { get; private set; }
            }

            [field: SerializeField]
            public Ability Toss { get; private set; }

            [field: SerializeField]
            public Ability PushUp { get; private set; }
        }

        public event Action OnTossed;
        public event Action OnPushedUp;

        private readonly Settings _settings;
        private readonly TransformComponent _transformComponent;
        private readonly HealthComponent _healthComponent;
        private readonly MoveComponent _moveComponent;
        private readonly LookComponent _lookComponent;
        private readonly GroundedComponent _groundedComponent;
        private readonly JumpComponent _jumpComponent;
        private readonly KnockbackComponent _knockbackComponent;

        private float _tossDelayLeft = -1f;
        private float _tossCooldownLeft;
        private float _pushUpDelayLeft = -1f;
        private float _pushUpCooldownLeft;

        public bool IsTossing => _tossDelayLeft >= 0 || _tossCooldownLeft > 0;
        public bool IsPushingUp => _pushUpDelayLeft >= 0 || _pushUpCooldownLeft > 0;

        public Character(
            Settings settings,
            TransformComponent transformComponent,
            HealthComponent healthComponent,
            MoveComponent moveComponent,
            LookComponent lookComponent,
            GroundedComponent groundedComponent,
            JumpComponent jumpComponent,
            KnockbackComponent knockbackComponent)
        {
            _settings = settings;
            _transformComponent = transformComponent;
            _healthComponent = healthComponent;
            _moveComponent = moveComponent;
            _lookComponent = lookComponent;
            _groundedComponent = groundedComponent;
            _jumpComponent = jumpComponent;
            _knockbackComponent = knockbackComponent;
        }

        void IInitializable.Initialize()
        {
            _moveComponent.SetCondition(this);
            _jumpComponent.SetCondition(this);
        }

        void IDisposable.Dispose()
        {
        }

        public void Move(Vector2 direction)
        {
            _moveComponent.Move(direction);

            if (direction.x != 0 && _healthComponent.IsAlive)
                _lookComponent.Look(direction.x);
        }

        public void Jump()
        {
            _jumpComponent.Jump();
        }

        public void Toss()
        {
            if (!_healthComponent.IsAlive)
                return;

            if (this.IsTossing || this.IsPushingUp)
                return;

            _tossDelayLeft = _settings.Toss.Delay;
        }

        public void PushUp()
        {
            if (!_healthComponent.IsAlive)
                return;

            if (!_groundedComponent.IsGrounded)
                return;

            if (this.IsTossing || this.IsPushingUp)
                return;

            _pushUpDelayLeft = _settings.PushUp.Delay;
        }

        void ITickable.Tick()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                Jump();

            if (Input.GetMouseButtonDown(0))
                Toss();

            if (Input.GetMouseButtonDown(1))
                PushUp();
        }

        void IFixedTickable.FixedTick()
        {
            TickMove();
            TickToss();
            TickPushUp();
        }

        private void TickMove()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            Move(new Vector2(horizontal, 0f));
        }

        private void TickToss()
        {
            if (_tossCooldownLeft > 0)
                _tossCooldownLeft -= Time.fixedDeltaTime;

            if (_tossDelayLeft < 0)
                return;

            _tossDelayLeft -= Time.fixedDeltaTime;
            if (_tossDelayLeft > 0)
                return;

            _tossDelayLeft = -1f;
            this.ApplyKnockback(_settings.Toss);
            _tossCooldownLeft = _settings.Toss.Cooldown;
            OnTossed?.Invoke();
        }

        private void TickPushUp()
        {
            if (_pushUpCooldownLeft > 0)
                _pushUpCooldownLeft -= Time.fixedDeltaTime;

            if (_pushUpDelayLeft < 0)
                return;

            _pushUpDelayLeft -= Time.fixedDeltaTime;
            if (_pushUpDelayLeft > 0)
                return;

            _pushUpDelayLeft = -1f;
            this.ApplyKnockback(_settings.PushUp);
            _pushUpCooldownLeft = _settings.PushUp.Cooldown;
            OnPushedUp?.Invoke();
        }

        private void ApplyKnockback(Settings.Ability ability)
        {
            Transform self = _transformComponent.Transform;
            float dirX = self.right.x >= 0 ? 1f : -1f;
            Vector2 origin = (Vector2) self.position
                + new Vector2(ability.OverlapOffset.x * dirX, ability.OverlapOffset.y);

            Collider2D[] hits = Physics2D.OverlapBoxAll(origin, ability.OverlapSize, 0f, ability.Mask);
            foreach (Collider2D hit in hits)
                _knockbackComponent.TryKnockback(hit, ability.Force);
        }

        bool MoveComponent.ICondition.Evaluate() =>
            _healthComponent.IsAlive;

        bool JumpComponent.ICondition.Evaluate() =>
            _healthComponent.IsAlive && _groundedComponent.IsGrounded;
    }
}
