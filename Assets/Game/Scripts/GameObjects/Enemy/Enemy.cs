using Fusion;
using Game.Core;
using UnityEngine;

namespace Game.GameObjects
{
    public class Enemy : NetworkBehaviour, MoveComponent.ICondition, IInteractableComponent
    {
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private MoveComponent _moveComponent;
        [SerializeField] private float _attackCooldown;

        private Vector3 _moveDirection;

        [Networked]
        public NetworkBool IsPortalReached { get; private set; }

        [Networked]
        private TickTimer AttackTimer { get; set; }
        
        public override void Spawned()
        {
            IsPortalReached = false;
            ResetTimer();
            
            _moveComponent.SetCondition(this);
        }

        bool MoveComponent.ICondition.IsMet()
            => _healthComponent.IsAlive;

        public override void FixedUpdateNetwork()
        {
            _moveComponent.Move(_moveDirection);
        }

        public void SetMoveDirection(Vector3 direction)
        {
            _moveDirection = direction;
        }

        public void Interact(GameObject interactor)
        {
            if(!Runner || !Runner.IsRunning)
                return;
            
            if (interactor.TryGetComponent(out Portal _) && 
                interactor.TryGetComponent(out TakeDamageComponent portalTakeDamage))
            {
                portalTakeDamage.TakeDamage(new TakeDamageArgs(1));
                IsPortalReached = true;
            }
            else if(interactor.TryGetComponent(out Hero _) && 
                    interactor.TryGetComponent(out TakeDamageComponent heroTakeDamage) && 
                    AttackTimer.Expired(Runner))
            {
                heroTakeDamage.TakeDamage(new TakeDamageArgs(1));
                ResetTimer();
            }
            else if(interactor.TryGetComponent(out Mine _) && 
                    interactor.TryGetComponent(out TakeDamageComponent mineTakeDamage))
            {
                mineTakeDamage.TakeDamage(new TakeDamageArgs(1));
                _healthComponent.Die();
            }
        }

        private void ResetTimer()
        {
            AttackTimer = TickTimer.CreateFromSeconds(Runner, _attackCooldown);
        }
    }
}
