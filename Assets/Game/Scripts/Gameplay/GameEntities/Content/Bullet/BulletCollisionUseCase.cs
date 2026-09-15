using UnityEngine;

namespace Game.Gameplay
{
    public static class BulletCollisionUseCase
    {
        public static bool TryFindHit(
            Vector3 position, 
            Vector3 direction, 
            float distance,
            float radius, 
            LayerMask collisionMask,
            Transform owner, 
            out Collider collider)
        {
            RaycastHit[] hits = Physics.SphereCastAll(position, radius, direction, distance,
                collisionMask, QueryTriggerInteraction.Ignore);

            collider = null;
            float closestDistance = float.MaxValue;
            
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.transform.IsChildOf(owner) || hit.distance >= closestDistance)
                    continue;

                collider = hit.collider;
                closestDistance = hit.distance;
            }

            return collider != null;
        }
    }
}
