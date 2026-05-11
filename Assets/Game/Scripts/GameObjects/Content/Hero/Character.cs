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

        public event Action OnPushed;
        public event Action OnBlownUp;

        private readonly Settings _settings;
        private readonly TransformComponent _transformComponent;
        private readonly HealthComponent _healthComponent;
        private readonly MoveComponent _moveComponent;
        private readonly LookComponent _lookComponent;
        private readonly GroundedComponent _groundedComponent;
        private readonly JumpComponent _jumpComponent;
        private readonly PushComponent _pushComponent;

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
            PushComponent pushComponent)
        {
            _settings = settings;
            _transformComponent = transformComponent;
            _healthComponent = healthComponent;
            _moveComponent = moveComponent;
            _lookComponent = lookComponent;
            _groundedComponent = groundedComponent;
            _jumpComponent = jumpComponent;
            _pushComponent = pushComponent;
        }

        void IInitializable.Initialize()
        {
            _moveComponent.SetCondition(this);
            _jumpComponent.SetCondition(this);
        }

        void IDisposable.Dispose()
        {
        }

        private void Jump()
        {
            _jumpComponent.Jump();
        }

        private void Toss()
        {
            if (!_healthComponent.IsAlive)
                return;

            if (IsTossing || IsPushingUp)
                return;

            _tossDelayLeft = _settings.Toss.Delay;
        }

        private void PushUp()
        {
            if (!_healthComponent.IsAlive)
                return;

            if (!_groundedComponent.IsGrounded)
                return;

            if (IsTossing || IsPushingUp)
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
            float deltaTime = Time.fixedDeltaTime;
            
            MoveTick(deltaTime);
            TossTick(deltaTime);
            PushUpTick(deltaTime);
        }

        private void MoveTick(float deltaTime)
        {
            float horizontal = Input.GetAxis("Horizontal");
            Vector2 direction = new Vector2(horizontal, 0f);
            
            _moveComponent.Move(direction, deltaTime);

            if (direction.x != 0 && _healthComponent.IsAlive)
                _lookComponent.Look(direction.x);
        }

        private void TossTick(float deltaTime)
        {
            if (_tossCooldownLeft > 0)
                _tossCooldownLeft -= deltaTime;

            if (_tossDelayLeft < 0)
                return;

            _tossDelayLeft -= deltaTime;
            if (_tossDelayLeft > 0)
                return;

            _tossDelayLeft = -1f;
            ApplyKnockback(_settings.Toss);
            _tossCooldownLeft = _settings.Toss.Cooldown;
            OnPushed?.Invoke();
        }

        private void PushUpTick(float deltaTime)
        {
            if (_pushUpCooldownLeft > 0)
                _pushUpCooldownLeft -= deltaTime;

            if (_pushUpDelayLeft < 0)
                return;

            _pushUpDelayLeft -= deltaTime;
            if (_pushUpDelayLeft > 0)
                return;

            _pushUpDelayLeft = -1f;
            ApplyKnockback(_settings.PushUp);
            _pushUpCooldownLeft = _settings.PushUp.Cooldown;
            OnBlownUp?.Invoke();
        }

        private void ApplyKnockback(Settings.Ability ability)
        {
            Transform self = _transformComponent.Transform;
            float dirX = self.right.x >= 0 ? 1f : -1f;
            Vector2 origin = (Vector2) self.position + new Vector2(ability.OverlapOffset.x * dirX, ability.OverlapOffset.y);

            Collider2D[] hits = Physics2D.OverlapBoxAll(origin, ability.OverlapSize, 0f, ability.Mask);
            
            foreach (Collider2D hit in hits)
                _pushComponent.TryPush(hit, ability.Force);
        }

        bool MoveComponent.ICondition.Evaluate() =>
            _healthComponent.IsAlive;

        bool JumpComponent.ICondition.Evaluate() =>
            _healthComponent.IsAlive && _groundedComponent.IsGrounded;
    }
}
