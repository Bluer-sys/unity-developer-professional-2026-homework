using Atomic.Elements;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class EnemyZone : MonoBehaviour
    {
        [SerializeField] private TriggerEvents _triggerEvents;
        [SerializeField] private GameEntity[] _enemies;

        private IGameEntity _target;

        private void OnEnable()
        {
            _triggerEvents.OnEntered += OnEntered;
            _triggerEvents.OnExited += OnExited;

            foreach (Collider collider in _triggerEvents.CurrentColliders)
                OnEntered(collider);
        }

        private void OnDisable()
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
            IGameEntity target = _target;

            if (target == null || !ReferenceEquals(collider.GetComponentInParent<GameEntity>(), target))
                return;

            foreach (Collider current in _triggerEvents.CurrentColliders)
            {
                if (current != collider &&
                    ReferenceEquals(current.GetComponentInParent<GameEntity>(), target))
                    return;
            }

            SetTarget(null);
        }

        private void OnDeath(DamageArgs damage)
        {
            SetTarget(null);
        }

        private void SetTarget(IGameEntity target)
        {
            IGameEntity previousTarget = _target;

            if (ReferenceEquals(previousTarget, target))
                return;

            if (previousTarget != null)
                previousTarget.GetDeathEvent().OnEvent -= OnDeath;

            _target = target;

            if (target != null)
                target.GetDeathEvent().OnEvent += OnDeath;

            foreach (GameEntity enemy in _enemies)
                if (target == null || enemy.IsAlive())
                    TargetUseCase.SetTarget(enemy, target);
        }
    }
}
