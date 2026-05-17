using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    public sealed class JumpComponent
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
        private readonly ICoroutineRunner _coroutineRunner;

        private ICondition _condition;
        private Coroutine _jumpCoroutine;
        
        public JumpComponent(Settings settings, RigidbodyComponent rigidbodyComponent, ICoroutineRunner coroutineRunner)
        {
            _settings = settings;
            _rigidbodyComponent = rigidbodyComponent;
            _coroutineRunner = coroutineRunner;
        }

        public void SetCondition(ICondition condition)
        {
            _condition = condition;
        }

        public void Jump()
        {
            if (_condition != null && !_condition.Evaluate())
                return;
            
            if(_jumpCoroutine != null)
                return;

            _jumpCoroutine = _coroutineRunner.StartCoroutine(JumpCoroutine());
        }

        private IEnumerator JumpCoroutine()
        {
            yield return  new WaitForSeconds(_settings.Delay);

            _rigidbodyComponent.Rigidbody.AddForce(Vector2.up * _settings.Force, ForceMode2D.Impulse);
            OnJumped?.Invoke();
            
            yield return  new WaitForSeconds(_settings.Cooldown);
            
            _jumpCoroutine = null;
        }
    }
}
