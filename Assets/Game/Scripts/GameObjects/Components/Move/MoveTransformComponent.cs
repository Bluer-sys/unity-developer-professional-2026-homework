using System;
using UnityEngine;

namespace Game
{
    public sealed class MoveTransformComponent
    {
        public interface ICondition
        {
            bool Evaluate();
        }

        [Serializable]
        public class Settings
        {
            [field: SerializeField]
            public float Speed { get; private set; }
        }

        private readonly Settings _settings;
        private readonly Transform _transform;
        private ICondition _condition;

        public Vector2 MoveDirection { get; private set; }

        public MoveTransformComponent(Settings settings, Transform transform)
        {
            _settings = settings;
            _transform = transform;
        }

        public void SetCondition(ICondition condition)
        {
            _condition = condition;
        }

        public void Move(Vector2 direction, float deltaTime)
        {
            if(_condition != null && !_condition.Evaluate() || direction == Vector2.zero)
            {
                MoveDirection = Vector2.zero;
                return;
            }

            _transform.Translate((Vector3) direction * (_settings.Speed * deltaTime), Space.World);

            MoveDirection = direction;
        }
        
        public void Move(Transform point, float deltaTime)
        {
            if (_condition != null && !_condition.Evaluate())
            {
                MoveDirection = Vector2.zero;
                return;
            }
            
            var direction = (point.position - _transform.position).normalized;
            Move(direction, deltaTime);
        }
    }
}
