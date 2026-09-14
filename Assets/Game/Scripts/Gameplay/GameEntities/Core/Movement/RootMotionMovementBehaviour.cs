using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class RootMotionMovementBehaviour : 
        IEntityInit<IGameEntity>, 
        IEntityEnable<IGameEntity>, 
        IEntityDisable<IGameEntity>
    {
        private Rigidbody _rigidbody;
        private ISignal<Vector3, float> _rootMotion;
        private ISignal<DamageArgs> _deathEvent;
        private IValue<int> _health;
        private IVariable<Vector3> _movementDirection;
        private IVariable<Vector3> _rotationDirection;
        private IValue<float> _rootMotionMultiplier;
        private IValue<float> _rotationSpeed;

        public void Init(IGameEntity entity)
        {
            _rigidbody = entity.GetRigidbody();
            _rootMotion = entity.GetRootMotion();
            _deathEvent = entity.GetDeathEvent();
            _health = entity.GetHealth();
            _movementDirection = entity.GetMovementDirection();
            _rotationDirection = entity.GetRotationDirection();
            _rootMotionMultiplier = entity.GetRootMotionMultiplier();
            _rotationSpeed = entity.GetRotationSpeed();
        }

        public void Enable(IGameEntity entity)
        {
            _rootMotion.OnEvent += OnRootMotion;
            _deathEvent.OnEvent += OnDeath;

            if (_health.Value == 0)
                Stop();
        }

        public void Disable(IGameEntity entity)
        {
            _rootMotion.OnEvent -= OnRootMotion;
            _deathEvent.OnEvent -= OnDeath;
            
            Stop();
        }

        private void OnRootMotion(Vector3 deltaPosition, float deltaTime)
        {
            if (_health.Value == 0)
            {
                Stop();
                return;
            }

            Vector3 velocity = MovementUseCase
                .GetRootMotionVelocity(_movementDirection.Value, deltaPosition, _rootMotionMultiplier.Value, deltaTime);
            
            velocity.y = _rigidbody.linearVelocity.y;
            _rigidbody.linearVelocity = velocity;
            _rigidbody.angularVelocity = Vector3.zero;

            Quaternion rotation = RotationUseCase
                .GetRotation(_rigidbody.rotation, _rotationDirection.Value, _rotationSpeed.Value, deltaTime);
            
            _rigidbody.MoveRotation(rotation);
        }

        private void OnDeath(DamageArgs damage)
        {
            Stop();
        }

        private void Stop()
        {
            _movementDirection.Value = Vector3.zero;
            _rotationDirection.Value = Vector3.zero;
            _rigidbody.linearVelocity = new Vector3(0, _rigidbody.linearVelocity.y, 0);
            _rigidbody.angularVelocity = Vector3.zero;
        }
    }
}
