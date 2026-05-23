using UnityEngine;

namespace Game
{
    public sealed class LookComponent
    {
        public interface ICondition
        {
            bool Evaluate();
        }
        
        private readonly TransformComponent _transformComponent;
        private ICondition _condition;

        public float CurrentDirection => _transformComponent.Transform.localScale.x > 0 ? 1 : -1;
        
        public LookComponent(TransformComponent transformComponent)
        {
            _transformComponent = transformComponent;
        }

        public void SetCondition(ICondition condition)
        {
            _condition = condition;
        }

        public void Look(Transform target)
        {
            if(_condition != null && !_condition.Evaluate())
                return;
            
            Vector2 direction = target.position - _transformComponent.Transform.position;
            Look(direction.x);
        }

        public void Look(float direction)
        {
            if (_condition != null && !_condition.Evaluate())
                return;
            
            float angle = direction > 0 ? 0 : 180;
            _transformComponent.Transform.eulerAngles = new Vector3(0, angle, 0);
        }
    }
}
