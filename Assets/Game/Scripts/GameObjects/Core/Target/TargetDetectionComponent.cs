using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Game.Core
{
    public class TargetDetectionComponent : NetworkBehaviour
    {
        [SerializeField] private float _radius;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private TargetComponent _targetComponent;

        private readonly Collider[] _colliders = new Collider[8];

        public override void Spawned()
        {
            DistanceComparer.Instance.Origin = transform.position;
        }

        public override void FixedUpdateNetwork()
        {
            if (Runner == null || !Runner.IsRunning)
                return;

            int count = Runner.GetPhysicsScene().OverlapSphere(
                    transform.position,
                    _radius,
                    _colliders,
                    _layerMask,
                    QueryTriggerInteraction.Collide
                );

            if (count < 0)
                return;
            
            Array.Sort(_colliders, 0, count, DistanceComparer.Instance);

            for (int i = 0; i < count; i++)
            {
                Collider col = _colliders[i];

                if(!IsTargetValid(col))
                    continue;
                
                _targetComponent.Target = col.GetComponentInParent<NetworkObject>();
                break;
            }
        }

        private bool IsTargetValid(Collider targetCollider)
        {
            if (targetCollider == null)
                return false;

            var obj = targetCollider.GetComponentInParent<NetworkObject>();

            if (obj == null || obj.Runner == null || !obj.Runner.IsRunning)
                return false;
            
            return obj.TryGetBehaviour(out HealthComponent targetHealth) && 
                   targetHealth.IsAlive;
        }
        
        private sealed class DistanceComparer : IComparer<Collider>
        {
            public static readonly DistanceComparer Instance = new();

            public Vector3 Origin;

            public int Compare(Collider a, Collider b)
            {
                float distA = (a.transform.position - Origin).sqrMagnitude;
                float distB = (b.transform.position - Origin).sqrMagnitude;
                return distA.CompareTo(distB);
            }
        }
    }
}
