using UnityEngine;

namespace Game.GameObjects
{
    public class MovementViewComponent : MonoBehaviour
    {
        [SerializeField] private ShipViewConfig _viewConfig;
        [SerializeField] private Transform _viewTransform;
        [SerializeField] private MovementComponent _movement;
        
        protected void LateUpdate()
        {
            AnimateMovement(Time.deltaTime);
        }

        private void AnimateMovement(float deltaTime)
        {
            Vector3 shipAngles = _viewTransform.localEulerAngles;
            shipAngles.x = _viewConfig.MoveRotationAngle * _movement.CurrentMoveDirection.y;
            shipAngles.y = _viewConfig.MoveRotationAngle / 2 * _movement.CurrentMoveDirection.x * -1f;

            Quaternion shipRotation = Quaternion.Euler(shipAngles);
            float t = _viewConfig.MoveSpeed * deltaTime;
            _viewTransform.localRotation = Quaternion.Lerp(_viewTransform.localRotation, shipRotation, t);
        }
    }
}
