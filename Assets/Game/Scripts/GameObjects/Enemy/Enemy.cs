using System;
using Fusion;
using Game.Core;
using UnityEngine;

namespace Game.GameObjects
{
    public class Enemy : NetworkBehaviour, MoveComponent.ICondition, IInteractableComponent
    {
        public event Action<NetworkObject> OnPortalReached;

        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private MoveComponent _moveComponent;
        [SerializeField] private float _attackCooldown;

        private Vector3 _moveDirection;

        [Networked, OnChangedRender(nameof(OnPortalReachedChanged))]
        private NetworkBool IsPortalReached { get; set; }

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
            if (interactor.TryGetComponent(out Portal _) && 
                interactor.TryGetComponent(out HealthComponent portalHealth))
            {
                portalHealth.Decrement(1);
                IsPortalReached = true;
            }
            else if(interactor.TryGetComponent(out Hero _) && 
                    interactor.TryGetComponent(out HealthComponent heroHealth) && 
                    AttackTimer.Expired(Runner))
            {
                heroHealth.Decrement(1);
                ResetTimer();
            }
        }

        private void OnPortalReachedChanged()
        {
            if(IsPortalReached)
                OnPortalReached?.Invoke(Object);
        }

        private void ResetTimer()
        {
            AttackTimer = TickTimer.CreateFromSeconds(Runner, _attackCooldown);
        }
    }
}
