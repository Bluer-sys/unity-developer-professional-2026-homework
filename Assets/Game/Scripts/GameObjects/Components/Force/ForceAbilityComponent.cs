using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    public sealed class ForceAbilityComponent
    {
        [Serializable]
        public class Settings
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
            public float OverlapSize { get; private set; }
            
            [field: SerializeField]
            public int OverlapMaxCount { get; private set; }

            [field: SerializeField]
            public LayerMask Mask { get; private set; }
        }

        public event Action OnPerformed;

        private readonly Settings _settings;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly TransformComponent _transformComponent;

        private Func<bool> _condition;
        private Coroutine _coroutine;
        
        public bool IsProcessing => _coroutine != null;
        
        public ForceAbilityComponent(
            Settings settings,
            ICoroutineRunner coroutineRunner,
            TransformComponent transformComponent)
        {
            _settings = settings;
            _coroutineRunner = coroutineRunner;
            _transformComponent = transformComponent;
        }

        public void SetCondition(Func<bool> condition)
        {
            _condition = condition;
        }

        public void Apply()
        {
            if (_condition != null && !_condition.Invoke())
                return;
            
            if(_coroutine != null)
                return;

            _coroutine = _coroutineRunner.StartCoroutine(ApplyCoroutine());
        }

        private IEnumerator ApplyCoroutine()
        {
            yield return new WaitForSeconds(_settings.Delay);

            ApplyForce();
            OnPerformed?.Invoke();
            
            yield return new WaitForSeconds(_settings.Cooldown);
            
            _coroutine = null;
        }

        private void ApplyForce()
        {
            var hits = Physics2D.OverlapCircleAll(_settings.Origin.position, _settings.OverlapSize, _settings.Mask);

            for (int i = 0; i < Mathf.Min(hits.Length, _settings.OverlapMaxCount); i++)
            {
                var hit = hits[i];
                var rb = hit?.attachedRigidbody;

                if (rb == null)
                    return;

                var self = _transformComponent.Transform;
                var dirX = Mathf.Sign(rb.transform.position.x - self.position.x);
                var force = new Vector2(_settings.Force.x * dirX, _settings.Force.y);
                
                rb.AddForce(force, ForceMode2D.Impulse);
            }
        }
    }
}
