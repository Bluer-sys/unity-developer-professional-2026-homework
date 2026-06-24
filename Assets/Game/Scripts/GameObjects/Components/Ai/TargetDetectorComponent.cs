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

        private readonly Settings _settings;
        private readonly Transform _transform;
        private readonly TargetComponent _targetComponent;

        public TargetDetectorComponent(
            Settings settings,
            Transform transform,
            TargetComponent targetComponent)
        {
            _settings = settings;
            _transform = transform;
            _targetComponent = targetComponent;
        }

        void IFixedTickable.FixedTick()
        {
            Vector2 origin = _transform.position;
            Collider2D hit = Physics2D.OverlapCircle(origin, _settings.Radius, _settings.Mask);
            Transform newTarget = hit ? hit.transform : null;

            _targetComponent.SetTarget(newTarget);
        }
    }
}
