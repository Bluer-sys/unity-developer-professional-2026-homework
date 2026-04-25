using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class PatrolComponent : IFixedTickable
    {
        [Serializable]
        public class Settings
        {
            [field: SerializeField]
            public Transform Left { get; private set; }

            [field: SerializeField]
            public Transform Right { get; private set; }
        }

        public event Action<int> OnDirectionChanged;

        private readonly Settings _settings;
        private readonly TransformComponent _transformComponent;
        private readonly MoveComponent _moveComponent;

        private int _direction = 1;
        private bool _enabled = true;

        public int Direction => _direction;

        public PatrolComponent(
            Settings settings,
            TransformComponent transformComponent,
            MoveComponent moveComponent)
        {
            _settings = settings;
            _transformComponent = transformComponent;
            _moveComponent = moveComponent;
        }

        public void Enable() => _enabled = true;
        public void Disable() => _enabled = false;

        void IFixedTickable.FixedTick()
        {
            if (!_enabled)
                return;

            float x = _transformComponent.Transform.position.x;

            if (_direction > 0 && x >= _settings.Right.position.x)
                this.SetDirection(-1);
            else if (_direction < 0 && x <= _settings.Left.position.x)
                this.SetDirection(1);

            _moveComponent.Move(new Vector2(_direction, 0));
        }

        private void SetDirection(int direction)
        {
            if (_direction == direction)
                return;

            _direction = direction;
            OnDirectionChanged?.Invoke(_direction);
        }
    }
}
