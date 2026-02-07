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
        
        public void Move(Vector2 direction)
        {
            Vector2 delta = direction * (_speed * Time.fixedDeltaTime);
            Vector2 newPosition = _rigidbody.position + delta;

            _rigidbody.MovePosition(newPosition);
            
            CurrentMoveDirection = direction;
            
            if(_bounds != null)
                transform.position = _bounds.ClampInBounds(transform.position);
        }
    }
}
