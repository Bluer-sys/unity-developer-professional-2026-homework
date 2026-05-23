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
        }

        private readonly Settings _settings;

        private Func<bool> _condition;
        private Coroutine _coroutine;
        
        public ForceTargetComponent(Settings settings)
        {
            _settings = settings;
        }

        public void SetCondition(Func<bool> condition)
        {
            _condition = condition;
        }

        public void ApplyForce(Rigidbody2D rb)
        {
            if (_condition != null && !_condition.Invoke())
                return;
            
            rb.AddForce(_settings.Force, ForceMode2D.Impulse);
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
