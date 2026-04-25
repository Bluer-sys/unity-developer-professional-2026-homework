using System;
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
        private readonly RigidbodyComponent _rigidbodyComponent;

        private ICondition _condition;
        private float _delayLeft = -1f;
        private float _cooldownLeft;

        public JumpComponent(Settings settings, RigidbodyComponent rigidbodyComponent)
        {
            _settings = settings;
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
            _rigidbodyComponent.Rigidbody.AddForce(Vector2.up * _settings.Force, ForceMode2D.Impulse);
            _cooldownLeft = _settings.Cooldown;
            OnJumped?.Invoke();
        }
    }
}
