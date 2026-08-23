using Fusion;
using UnityEngine;

namespace Game
{
    public sealed class BoxInteractComponent : NetworkBehaviour
    {
        private static readonly Collider[] Colliders = new Collider[32];

        [SerializeField] private BoxCollider _collider;
        [SerializeField] private LayerMask _layerMask;

        public override void FixedUpdateNetwork()
        {
            _collider.GetCenterExtentsAndRotation(out Vector3 center, out Vector3 halfExtents, out Quaternion rotation);

            PhysicsScene scene = Runner.GetPhysicsScene();
            int count = scene.OverlapBox(
                    center,
                    halfExtents,
                    Colliders,
                    rotation,
                    _layerMask,
                    QueryTriggerInteraction.Collide
                );

            for (int i = 0; i < count; i++)
            {
                Collider col = Colliders[i];
                var interactable = col.GetComponentInParent<IInteractableComponent>();

                interactable?.Interact(gameObject);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (_collider == null)
                return;

            _collider.GetCenterExtentsAndRotation(out var center, out var halfExtents, out var rotation);

            Gizmos.color = Color.cyan;
            Gizmos.matrix = Matrix4x4.TRS(center, rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, halfExtents * 2f);
            Gizmos.matrix = Matrix4x4.identity;
        }
#endif
    }
}
