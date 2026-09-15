using System.Collections.Generic;
using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class EnemyZoneBehaviour :
        IEntityInit<IGameEntity>,
        IEntityEnable<IGameEntity>,
        IEntityDisable<IGameEntity>
    {
        private readonly TriggerEvents _triggerEvents;
        private readonly IReadOnlyList<GameEntity> _enemies;
        private IVariable<IGameEntity> _target;

        public EnemyZoneBehaviour(TriggerEvents triggerEvents, IReadOnlyList<GameEntity> enemies)
        {
            _triggerEvents = triggerEvents;
            _enemies = enemies;
        }

        public void Init(IGameEntity entity)
        {
            _target = entity.GetTarget();
        }

        public void Enable(IGameEntity entity)
        {
            _triggerEvents.OnEntered += OnEntered;
            _triggerEvents.OnExited += OnExited;

            foreach (Collider collider in _triggerEvents.CurrentColliders)
                OnEntered(collider);
        }

        public void Disable(IGameEntity entity)
        {
            _triggerEvents.OnEntered -= OnEntered;
            _triggerEvents.OnExited -= OnExited;
            
            SetTarget(null);
        }

        private void OnEntered(Collider collider)
        {
            GameEntity target = collider.GetComponentInParent<GameEntity>();

            if (target != null && target.HasPlayerTag() && target.IsAlive())
                SetTarget(target);
        }

        private void OnExited(Collider collider)
        {
            IGameEntity target = _target.Value;

            if (target == null || !ReferenceEquals(collider.GetComponentInParent<GameEntity>(), target))
                return;

            foreach (Collider current in _triggerEvents.CurrentColliders)
                if (current != collider && 
                    ReferenceEquals(current.GetComponentInParent<GameEntity>(), target))
                    return;

            SetTarget(null);
        }

        private void OnDeath(DamageArgs damage)
        {
            SetTarget(null);
        }

        private void SetTarget(IGameEntity target)
        {
            IGameEntity previousTarget = _target.Value;

            if (ReferenceEquals(previousTarget, target))
                return;

            if (previousTarget != null)
                previousTarget.GetDeathEvent().OnEvent -= OnDeath;

            _target.Value = target;

            if (target != null)
                target.GetDeathEvent().OnEvent += OnDeath;

            foreach (GameEntity enemy in _enemies)
                if (target == null || enemy.IsAlive())
                    TargetUseCase.SetTarget(enemy, target);
        }
    }
}
