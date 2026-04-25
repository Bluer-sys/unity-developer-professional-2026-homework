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
        private readonly TransformComponent _transformComponent;

        private Transform _target;

        public Transform Target => _target;
        public bool HasTarget => _target != null;

        public TargetDetectorComponent(Settings settings, TransformComponent transformComponent)
        {
            _settings = settings;
            _transformComponent = transformComponent;
        }

        void IFixedTickable.FixedTick()
        {
            Vector2 origin = _transformComponent.Transform.position;
            Collider2D hit = Physics2D.OverlapCircle(origin, _settings.Radius, _settings.Mask);
            Transform newTarget = hit ? hit.transform : null;

            if (newTarget != null && _target == null)
            {
                _target = newTarget;
                OnDetected?.Invoke(_target);
            }
            else if (newTarget == null && _target != null)
            {
                _target = null;
                OnLost?.Invoke();
            }
            else
            {
                _target = newTarget;
            }
        }
    }
}
