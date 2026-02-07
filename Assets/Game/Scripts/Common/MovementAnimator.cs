using UnityEngine;

namespace Game.Common
{
    public class MovementAnimator : MonoBehaviour
    {
        [SerializeField] private Transform _viewTransform;
        
        private MovementComponent _movement;
        private float _moveRotationAngle;
        private float _animationSpeed;

        public void Construct(MovementComponent movement,
                              float moveRotationAngle, 
                              float animationSpeed)
        {
            _movement = movement;
            _moveRotationAngle = moveRotationAngle;
            _animationSpeed = animationSpeed;
        }
        
        protected void LateUpdate()
        {
            AnimateMovement(Time.deltaTime);
        }

        private void AnimateMovement(float deltaTime)
        {
            Vector3 shipAngles = _viewTransform.localEulerAngles;
            shipAngles.x = _moveRotationAngle * _movement.CurrentMoveDirection.y;
            shipAngles.y = _moveRotationAngle / 2 * _movement.CurrentMoveDirection.x * -1f;

            Quaternion shipRotation = Quaternion.Euler(shipAngles);
            float t = _animationSpeed * deltaTime;
            _viewTransform.localRotation = Quaternion.Lerp(_viewTransform.localRotation, shipRotation, t);
        }
    }
}
