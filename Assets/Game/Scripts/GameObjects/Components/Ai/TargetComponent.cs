using System;
using UnityEngine;

namespace Game
{
    public sealed class TargetComponent
    {
        public event Action<Transform> OnDetected;
        public event Action OnLost;
        
        public Transform Target { get; private set; }

        public bool HasTarget => Target != null;

        public void SetTarget(Transform target)
        {
            if (target != null && !HasTarget)
            {
                Target = target;
                OnDetected?.Invoke(target);
            }
            else if (target == null && HasTarget)
            {
                Target = null;
                OnLost?.Invoke();
            }
            else
            {
                Target = target;
            }
        }
    }
}
