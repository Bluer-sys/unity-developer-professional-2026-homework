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
        private readonly TransformComponent _transformComponent;
        private ICondition _condition;

        public Vector2 MoveDirection { get; private set; }

        public MoveTransformComponent(Settings settings, TransformComponent transformComponent)
        {
            _settings = settings;
            _transformComponent = transformComponent;
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

            _transformComponent.Transform.Translate((Vector3) direction * (_settings.Speed * deltaTime), Space.World);

            MoveDirection = direction;
        }
        
        public void Move(Transform point, float deltaTime)
        {
            if (_condition != null && !_condition.Evaluate())
            {
                MoveDirection = Vector2.zero;
                return;
            }
            
            var direction = (point.position - _transformComponent.Transform.position).normalized;
            Move(direction, deltaTime);
        }
    }
}
