using System;
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
        
        public event Action OnFireStarted;
        
        [SerializeField] private TargetComponent _targetComponent;
        [SerializeField] private ProjectileWorld _projectileWorld;
        [SerializeField] private Transform _firePoint;
        [SerializeField] private float _fireCooldown;
        [SerializeField] private float _fireDelay;
        
        private ICondition _condition;
        
        [Networked, OnChangedRender(nameof(OnFireStartedHandler))]
        private bool IsFireStarted { get; set; }

        [Networked]
        private TickTimer FireTimestamp { get; set; }

        public override void FixedUpdateNetwork()
        {
            if(Runner == null || !Runner.IsRunning)
                return;

            if(!HasStateAuthority)
                return;

            ProcessFire();
        }

        public void SetCondition(ICondition condition) =>
            _condition = condition;

        private void ProcessFire()
        {
            if (!IsFireStarted)
            {
                if (_condition != null && !_condition.IsMet())
                    return;

                if (_targetComponent.Target == null)
                    return;

                // Cooldown
                if (!FireTimestamp.ExpiredOrNotRunning(Runner) && !IsFireStarted)
                    return;

                if (!IsFireStarted)
                {
                    ResetDelay();
                    IsFireStarted = true;
                }
            }
            else
            {
                // Delay
                if (!FireTimestamp.ExpiredOrNotRunning(Runner))
                    return;

                Fire();
                ResetCooldown();
                IsFireStarted = false;
            }
        }

        private void Fire()
        {
            _projectileWorld.TrySpawn(_firePoint.position, _firePoint.rotation);
        }

        private void ResetCooldown()
        {
            FireTimestamp = TickTimer.CreateFromSeconds(Runner, _fireCooldown);
        }
        
        private void ResetDelay()
        {
            FireTimestamp = TickTimer.CreateFromSeconds(Runner, _fireDelay);
        }

        private void OnFireStartedHandler()
        {
            if(IsFireStarted)
                OnFireStarted?.Invoke();
        }
    }
}
