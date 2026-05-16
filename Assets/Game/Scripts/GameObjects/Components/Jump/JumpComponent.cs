using System;
using Game.Scripts;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class JumpComponent : IFixedTickable
    {
        public interface ICondition
        {
            bool Evaluate();
        }

        [Serializable]
        public class Settings
        {
            [field: SerializeField]
            public float Force { get; private set; }

            [field: SerializeField]
            public float Cooldown { get; private set; }

            [field: SerializeField]
            public float Delay { get; private set; }
        }

        public event Action OnJumped;

        private readonly Settings _settings;
        private readonly PushComponent _pushComponent;
        private readonly RigidbodyComponent _rigidbodyComponent;

        private readonly UnityTimer _delayTimer;
        private readonly UnityTimer _cooldownTimer;
        
        private ICondition _condition;
        private float _delayLeft = -1f;
        private float _cooldownLeft;

        public JumpComponent(Settings settings, PushComponent pushComponent, RigidbodyComponent rigidbodyComponent)
        {
            _settings = settings;
            _pushComponent = pushComponent;
            _rigidbodyComponent = rigidbodyComponent;
        }

        public void SetCondition(ICondition condition)
        {
            _condition = condition;
        }

        public void Jump()
        {
            if (_cooldownLeft > 0)
                return;

            if (_delayLeft >= 0)
                return;

            if (_condition != null && !_condition.Evaluate())
                return;

            _delayLeft = _settings.Delay;
        }

        void IFixedTickable.FixedTick()
        {
            if (_cooldownLeft > 0)
                _cooldownLeft -= Time.fixedDeltaTime;

            if (_delayLeft < 0)
                return;

            _delayLeft -= Time.fixedDeltaTime;
            if (_delayLeft > 0)
                return;

            _delayLeft = -1f;
            _pushComponent.TryPush(_rigidbodyComponent.Rigidbody, Vector2.up * _settings.Force);
            _cooldownLeft = _settings.Cooldown;
            OnJumped?.Invoke();
        }
    }
}
