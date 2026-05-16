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
            Vector2 direction = target.position - _transformComponent.Transform.position;
            Vector2 directionN = direction.normalized;
            
            Look(directionN.x);
        }

        public void Look(float direction)
        {
            if(_condition == null || !_condition.Evaluate())
                return;
            
            _transformComponent.Transform.localScale = new Vector3(direction > 0 ? 1 : -1, 1, 1);
        }
    }
}
