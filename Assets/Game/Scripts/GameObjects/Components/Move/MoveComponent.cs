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
        private ICondition _condition;
		
        public MoveComponent(Settings settings)
        {
            _settings = settings;
        }
		
        public void SetCondition(ICondition condition)
        {
            _condition = condition;
        }

        public void Move(Vector2 direction)
        {
            if (direction != Vector2.zero && _condition.Evaluate()) 
                this.transform.Translate((Vector3) direction * _settings.Speed * Time.fixedDeltaTime);
        }
    }
}
