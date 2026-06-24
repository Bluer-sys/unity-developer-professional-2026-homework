using UnityEngine;

namespace Game
{
    public sealed class LookComponent
    {
        public interface ICondition
        {
            bool Evaluate();
        }
        
        private readonly Transform _transform;
        private ICondition _condition;

        public float CurrentDirection => _transform.localScale.x > 0 ? 1 : -1;
        
        public LookComponent(Transform transform)
        {
            _transform = transform;
        }

        public void SetCondition(ICondition condition)
        {
            _condition = condition;
        }

        public void Look(Transform target)
        {
            if(_condition != null && !_condition.Evaluate())
                return;
            
            Vector2 direction = target.position - _transform.position;
            Look(direction.x);
        }

        public void Look(float direction)
        {
            if (_condition != null && !_condition.Evaluate())
                return;
            
            float angle = direction > 0 ? 0 : 180;
            _transform.eulerAngles = new Vector3(0, angle, 0);
        }
    }
}
