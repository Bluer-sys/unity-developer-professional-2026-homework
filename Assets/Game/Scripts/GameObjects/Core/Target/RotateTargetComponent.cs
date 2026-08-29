using Fusion;
using Game.Projectiles;
using UnityEngine;

namespace Game.Core
{
    public class RotateTargetComponent : NetworkBehaviour
    {
        public interface ICondition
        {
            bool IsMet();
        }
        
        [SerializeField] private TargetComponent _targetComponent;
        
        private ICondition _condition;

        public override void FixedUpdateNetwork()
        {
            if(Runner == null || !Runner.IsRunning)
                return;

            if (_condition != null && !_condition.IsMet())
                return;
                
            if(_targetComponent.Target == null)
                return;
            
            Rotate();
        }

        public void SetCondition(ICondition condition)
        {
            _condition = condition;
        }

        private void Rotate()
        {
            transform.rotation = Quaternion.LookRotation(_targetComponent.Target.transform.position - transform.position);
        }
    }
}
