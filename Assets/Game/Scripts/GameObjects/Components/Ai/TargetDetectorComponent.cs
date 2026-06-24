using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class TargetDetectorComponent : IFixedTickable
    {
        [Serializable]
        public class Settings
        {
            [field: SerializeField]
            public float Radius { get; private set; }

            [field: SerializeField]
            public LayerMask Mask { get; private set; }
        }

        public event Action<Transform> OnDetected;
        public event Action OnLost;

        private readonly Settings _settings;
        private readonly Transform _transform;

        public Transform Target { get; private set; }

        public bool HasTarget => Target != null;

        public TargetDetectorComponent(Settings settings, Transform transform)
        {
            _settings = settings;
            _transform = transform;
        }

        void IFixedTickable.FixedTick()
        {
            Vector2 origin = _transform.position;
            Collider2D hit = Physics2D.OverlapCircle(origin, _settings.Radius, _settings.Mask);
            Transform newTarget = hit ? hit.transform : null;

            if (newTarget != null && Target == null)
            {
                Target = newTarget;
                OnDetected?.Invoke(Target);
            }
            else if (newTarget == null && Target != null)
            {
                Target = null;
                OnLost?.Invoke();
            }
            else
            {
                Target = newTarget;
            }
        }
    }
}
