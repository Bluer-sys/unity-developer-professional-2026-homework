using UnityEngine;

namespace Game.Common
{
    public class MovementComponent : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        private float _speed;
        
        public Vector2 CurrentMoveDirection { get; private set; }

        public void Construct(Rigidbody2D rigidbody, float speed)
        {
            _rigidbody = rigidbody;
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

        public void SetSpeed(float speed)
        {
            _speed = speed;
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
        }
    }
}
