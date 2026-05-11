using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class PatrolComponent : IInitializable, IFixedTickable
    {
        [Serializable]
        public class Settings
        {
            [field: SerializeField]
            public Transform FirstPoint { get; private set; }

            [field: SerializeField]
            public Transform SecondPoint { get; private set; }
        }

        private readonly Settings _settings;
        private readonly MoveComponent _moveComponent;
        private readonly TransformComponent _transformComponent;

        private bool _enabled = true;
        private Transform _targetPoint;

        public PatrolComponent(
            Settings settings,
            MoveComponent moveComponent,
            TransformComponent transformComponent)
        {
            _settings = settings;
            _moveComponent = moveComponent;
            _transformComponent = transformComponent;
        }

        public void Enable() => _enabled = true;
        public void Disable() => _enabled = false;

        void IInitializable.Initialize()
        {
            _targetPoint = _settings.SecondPoint;
        }

        void IFixedTickable.FixedTick()
        {
            if (!_enabled)
                return;

            _moveComponent.Move(_targetPoint, Time.fixedDeltaTime);

            float sqrMagnitude = (_transformComponent.Transform.position - _targetPoint.position).sqrMagnitude;
            
            if(sqrMagnitude < 0.1f)
                SwitchPoint();
        }

        private void SwitchPoint()
        {
            Transform first = _settings.FirstPoint;
            Transform second = _settings.SecondPoint;

            if (_targetPoint == first)
                _targetPoint = second;

            else if (_targetPoint == second)
                _targetPoint = first;
        }
    }
}
