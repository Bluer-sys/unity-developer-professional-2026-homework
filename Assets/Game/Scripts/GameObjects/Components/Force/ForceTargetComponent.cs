using System;
using UnityEngine;

namespace Game
{
    public sealed class ForceTargetComponent
    {
        [Serializable]
        public class Settings
        {
            [field: SerializeField]
            public Vector2 Force { get; private set; }
            
            [field: SerializeField]
            public bool RelativeSelf { get; private set; }
        }

        private readonly Settings _settings;
        private readonly Transform _transform;

        public event Action OnPerformed;

        private Func<bool> _condition;
        private Coroutine _coroutine;
        
        public ForceTargetComponent(
            Settings settings,
            Transform transform)
        {
            _settings = settings;
            _transform = transform;
        }

        public void SetCondition(Func<bool> condition)
        {
            _condition = condition;
        }

        public void ApplyForce(Rigidbody2D rb)
        {
            if (_condition != null && !_condition.Invoke())
                return;

            Vector2 force = _settings.Force;
            
            if (_settings.RelativeSelf)
            {
                var self = _transform;
                var dirX = Mathf.Sign(rb.transform.position.x - self.position.x);
                
                force = new Vector2(_settings.Force.x * dirX, _settings.Force.y);
            }
            
            rb.AddForce(force, ForceMode2D.Impulse);
            OnPerformed?.Invoke();
        }

        public void ApplyForce(Collider2D collider)
        {
            if(_condition != null && !_condition.Invoke())
                return;
            
            if (collider == null || collider.attachedRigidbody == null)
                return;

            ApplyForce(collider.attachedRigidbody);
        }
    }
}
