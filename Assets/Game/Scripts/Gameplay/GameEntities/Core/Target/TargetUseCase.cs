using System.Buffers;
using Atomic.Elements;
using UnityEngine;

namespace Game.Gameplay
{
    public static class TargetUseCase
    {
        public static void SetTarget(IGameEntity entity, IGameEntity target)
        {
            IVariable<IGameEntity> currentTarget = entity.GetTarget();

            if (currentTarget.Value == target)
                return;

            currentTarget.Value = target;
            AttackUseCase.Cancel(entity);
            entity.GetMovementDirection().Value = Vector3.zero;
            entity.GetRotationDirection().Value = Vector3.zero;
        }

        public static bool HasTarget(IGameEntity entity)
        {
            IGameEntity target = entity.GetTarget().Value;

            return target != null && entity.GetTargetPredicate().Invoke(target);
        }

        public static bool IsInAttackRange(IGameEntity entity)
        {
            return HasTarget(entity) && IsInRange(entity, entity.GetTarget().Value,
                entity.GetWeapon().GetAttackDistance().Value);
        }

        public static bool IsInRange(IGameEntity entity, IGameEntity target, float distance)
        {
            Vector3 offset = target.GetTransform().position - entity.GetTransform().position;

            return offset.sqrMagnitude <= distance * distance;
        }

        public static bool TryFindClosest(Vector3 center, float radius, LayerMask layerMask,
            IPredicate<IGameEntity> predicate, out IGameEntity target)
        {
            Collider[] colliders = ArrayPool<Collider>.Shared.Rent(32);
            int count = Physics.OverlapSphereNonAlloc(center, radius, colliders, layerMask,
                QueryTriggerInteraction.Collide);

            target = null;
            float closestDistance = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                GameEntity candidate = colliders[i].GetComponentInParent<GameEntity>();

                if (candidate == null || !predicate.Invoke(candidate))
                    continue;

                float distance = (candidate.GetTransform().position - center).sqrMagnitude;

                if (distance >= closestDistance)
                    continue;

                closestDistance = distance;
                target = candidate;
            }

            ArrayPool<Collider>.Shared.Return(colliders, true);

            return target != null;
        }
    }
}
