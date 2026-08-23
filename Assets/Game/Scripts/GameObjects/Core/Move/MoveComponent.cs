using System;
using Fusion;
using UnityEngine;

namespace Game.Core
{
    public class MoveComponent : NetworkBehaviour, IBeforeTick
    {
        public interface ICondition
        {
            public bool IsMet();
        }
        
        public event Action OnStateChange;
        
        [SerializeField]
        private float _moveSpeed;
        
        [SerializeField]
        private float _angularSpeed;

        private ICondition _condition;
        
        [Networked, OnChangedRender(nameof(StateChanged))]
        public NetworkBool IsMoving { get; private set; }

        public void Move(Vector3 direction)
        {
            if(_condition != null && !_condition.IsMet())
                return;
            
            if(direction == Vector3.zero)
                return;
            
            float deltaTime = Time.fixedDeltaTime;
            UpdateRotation(direction, deltaTime);
            UpdatePosition(direction, deltaTime);

            IsMoving = true;
        }

        public void Stop()
        {
            IsMoving = false;
        }

        public void BeforeTick()
        {
            Stop();
        }

        public void SetCondition(ICondition condition)
        {
            _condition = condition;
        }

        private void UpdateRotation(Vector3 direction, float deltaTime)
        {
            Quaternion current = transform.rotation;
            Quaternion target = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(current, target, _angularSpeed * deltaTime);
        }

        private void UpdatePosition(Vector3 direction, float deltaTime)
        {
            float moveSpeed = _moveSpeed;
            transform.position += direction * deltaTime * moveSpeed;
        }

        private void StateChanged()
        {
            OnStateChange?.Invoke();
        }
    }
}
