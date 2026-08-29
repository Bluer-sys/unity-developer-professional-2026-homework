using Fusion;
using Game.Projectiles;
using UnityEngine;

namespace Game.Core
{
    public class FireTargetComponent : NetworkBehaviour
    {
        public interface ICondition
        {
            bool IsMet();
        }
        
        [SerializeField] private TargetComponent _targetComponent;
        [SerializeField] private ProjectileWorld _projectileWorld;
        [SerializeField] private Transform _firePoint;
        [SerializeField] private float _fireCooldown;
        
        private ICondition _condition;

        [Networked]
        private TickTimer FireCooldown { get; set; }

        public override void FixedUpdateNetwork()
        {
            if(Runner == null || !Runner.IsRunning)
                return;

            if(!HasStateAuthority)
                return;

            if (_condition != null && !_condition.IsMet())
                return;
            
            if(_targetComponent.Target == null)
                return;
            
            if(!FireCooldown.ExpiredOrNotRunning(Runner))
                return;
            
            Fire();
            ResetTimer();
        }

        public void SetCondition(ICondition condition)
        {
            _condition = condition;
        }
        
        private void Fire()
        {
            _projectileWorld.TrySpawn(_firePoint.position, _firePoint.rotation);
        }

        private void ResetTimer()
        {
            FireCooldown = TickTimer.CreateFromSeconds(Runner, _fireCooldown);
        }
    }
}
