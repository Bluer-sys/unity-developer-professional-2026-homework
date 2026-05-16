using System;
using Game.Scripts;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class Character :
        IInitializable,
        ITickable,
        IFixedTickable,
        MoveComponent.ICondition,
        JumpComponent.ICondition,
        LookComponent.ICondition
    {
        [Serializable]
        public class Settings
        {
            [Serializable]
            public class Ability : ISerializationCallbackReceiver
            {
                [field: SerializeField]
                public Transform Origin { get; private set; }
                
                [field: SerializeField]
                public Vector2 Force { get; private set; }

                [field: SerializeField]
                public float Cooldown { get; private set; }

                [field: SerializeField]
                public float Delay { get; private set; }

                [field: SerializeField]
                public Vector2 OverlapSize { get; private set; }

                [field: SerializeField]
                public LayerMask Mask { get; private set; }
                
                public ITimer CooldownTimer { get; private set; }
                
                public ITimer DelayTimer { get; private set; }
                
                public bool InProcess => !CooldownTimer.IsFinished || !DelayTimer.IsFinished;

                public void OnBeforeSerialize() {}
                public void OnAfterDeserialize()
                {
                    CooldownTimer = new UnityTimer(Cooldown);
                    DelayTimer = new UnityTimer(Delay);
                }
            }

            [field: SerializeField]
            public Ability Push { get; private set; }

            [field: SerializeField]
            public Ability BlowUp { get; private set; }
        }

        public event Action OnPushed;
        public event Action OnBlownUp;

        private readonly Settings _settings;
        private readonly HealthComponent _healthComponent;
        private readonly MoveComponent _moveComponent;
        private readonly LookComponent _lookComponent;
        private readonly GroundedComponent _groundedComponent;
        private readonly JumpComponent _jumpComponent;
        private readonly PushComponent _pushComponent;

        public Character(
            Settings settings,
            HealthComponent healthComponent,
            MoveComponent moveComponent,
            LookComponent lookComponent,
            GroundedComponent groundedComponent,
            JumpComponent jumpComponent,
            PushComponent pushComponent)
        {
            _settings = settings;
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
            _lookComponent.SetCondition(this);
        }

        void ITickable.Tick()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _jumpComponent.Jump();

            if (!_settings.Push.InProcess && 
                !_settings.BlowUp.InProcess)
            {
                if (Input.GetMouseButtonDown(0))
                    _settings.Push.DelayTimer.Restart();

                if (Input.GetMouseButtonDown(1))
                    _settings.BlowUp.DelayTimer.Restart();
            }
        }

        void IFixedTickable.FixedTick()
        {
            MoveTick(Time.deltaTime);
            PushTick();
            BlowUpTick();
        }

        private void MoveTick(float deltaTime)
        {
            float horizontal = Input.GetAxis("Horizontal");
            Vector2 direction = new Vector2(horizontal, 0f);
            
            _moveComponent.Move(direction, deltaTime);

            if (direction.x != 0)
                _lookComponent.Look(direction.x);
        }

        private void PushTick()
        {
            var delayTimer = _settings.Push.DelayTimer;

            if(!delayTimer.IsActive)
                return;
            
            if(!delayTimer.IsFinished)
                return;
            
            ApplyAbility(_settings.Push);
            OnPushed?.Invoke();
            
            delayTimer.Stop();
            _settings.Push.CooldownTimer.Restart();
        }

        private void BlowUpTick()
        {
            var delayTimer = _settings.BlowUp.DelayTimer;

            if (!delayTimer.IsActive)
                return;

            if (!delayTimer.IsFinished)
                return;

            ApplyAbility(_settings.BlowUp);
            OnBlownUp?.Invoke();

            delayTimer.Stop();
            _settings.BlowUp.CooldownTimer.Restart();
        }

        private void ApplyAbility(Settings.Ability ability)
        {
            Collider2D[] hits = Physics2D.OverlapBoxAll(ability.Origin.position, ability.OverlapSize, 0f, ability.Mask);
            
            foreach (Collider2D hit in hits)
                _pushComponent.TryPush(hit, ability.Force);
        }

        bool MoveComponent.ICondition.Evaluate() => _healthComponent.IsAlive;
        bool JumpComponent.ICondition.Evaluate() => _healthComponent.IsAlive && _groundedComponent.IsGrounded;
        bool LookComponent.ICondition.Evaluate() => _healthComponent.IsAlive;
    }
}
