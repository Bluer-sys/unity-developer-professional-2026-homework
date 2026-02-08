using Modules.Utils;
using UnityEngine;

namespace Game.Common
{
    public class MovementComponent : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        private TransformBounds _bounds;
        private float _speed;
        
        public Vector2 CurrentMoveDirection { get; private set; }

        public void Construct(Rigidbody2D rigidbody, TransformBounds bounds, float speed)
        {
            _rigidbody = rigidbody;
            _bounds = bounds;
            _speed = speed;
        }

        private void FixedUpdate()
        {
            Move(Time.fixedDeltaTime);
        }

        public void SetDirection(Vector2 direction)
        {
            CurrentMoveDirection = direction;
        }

        public void ResetDirection()
        {
            CurrentMoveDirection = Vector2.zero;
        }

        private void Move(float deltaTime)
        {
            Vector2 delta = CurrentMoveDirection * (_speed * deltaTime);
            Vector2 newPosition = _rigidbody.position + delta;

            _rigidbody.MovePosition(newPosition);
            
            if(_bounds != null)
                transform.position = _bounds.ClampInBounds(transform.position);
        }
    }
}
