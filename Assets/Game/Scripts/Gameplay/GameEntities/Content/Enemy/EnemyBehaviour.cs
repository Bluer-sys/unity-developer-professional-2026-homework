using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class EnemyBehaviour :
        IEntityInit<IGameEntity>,
        IEntityEnable<IGameEntity>,
        IEntityDisable<IGameEntity>,
        IEntityFixedTick<IGameEntity>
    {
        private IGameEntity _entity;
        private Rigidbody _rigidbody;
        private IValue<int> _health;
        private IValue<IGameEntity> _target;
        private IVariable<Vector3> _movementDirection;
        private IVariable<Vector3> _rotationDirection;
        private ICooldown _attackDuration;
        private IAction _attackRequest;
        private ISignal<DamageArgs> _deathEvent;

        public void Init(IGameEntity entity)
        {
            _entity = entity;
            _rigidbody = entity.GetRigidbody();
            _health = entity.GetHealth();
            _target = entity.GetTarget();
            _movementDirection = entity.GetMovementDirection();
            _rotationDirection = entity.GetRotationDirection();
            _attackDuration = entity.GetAttackDuration();
            _attackRequest = entity.GetAttackRequest();
            _deathEvent = entity.GetDeathEvent();
        }

        public void Enable(IGameEntity entity)
        {
            _deathEvent.OnEvent += OnDeath;
        }

        public void Disable(IGameEntity entity)
        {
            _deathEvent.OnEvent -= OnDeath;
            TargetUseCase.SetTarget(entity, null);
        }

        public void FixedTick(IGameEntity entity, float deltaTime)
        {
            IGameEntity target = _target.Value;

            if (_health.Value == 0 || target == null || !target.IsAlive())
            {
                TargetUseCase.SetTarget(entity, null);
                _movementDirection.Value = Vector3.zero;
                _rotationDirection.Value = Vector3.zero;
                return;
            }

            Vector3 direction = target.GetRigidbody().position - _rigidbody.position;
            direction.y = 0;
            direction.Normalize();
            _rotationDirection.Value = direction;

            bool isInRange = TargetUseCase.IsInAttackRange(entity);
            _movementDirection.Value = _attackDuration.IsCompleted() && !isInRange ? direction : Vector3.zero;

            if (isInRange && _attackDuration.IsCompleted())
                _attackRequest.Invoke();
        }

        private void OnDeath(DamageArgs damage)
        {
            TargetUseCase.SetTarget(_entity, null);
        }
    }
}
