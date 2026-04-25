using System;
using UnityEngine;

namespace Game
{
    public sealed class MoveComponent
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

        public MoveComponent(Settings settings, TransformComponent transformComponent)
        {
            _settings = settings;
            _transformComponent = transformComponent;
        }

        public void SetCondition(ICondition condition)
        {
            _condition = condition;
        }

        public void Move(Vector2 direction)
        {
            if (direction == Vector2.zero)
                return;

            if (_condition != null && !_condition.Evaluate())
                return;

            _transformComponent.Transform.Translate((Vector3) direction * _settings.Speed * Time.fixedDeltaTime);
        }
    }
}
