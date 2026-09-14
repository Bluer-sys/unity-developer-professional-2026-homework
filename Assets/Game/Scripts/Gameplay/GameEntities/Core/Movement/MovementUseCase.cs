using UnityEngine;

namespace Game.Gameplay
{
    public static class MovementUseCase
    {
        public static Vector3 GetRootMotionVelocity(Vector3 direction, Vector3 deltaPosition, float multiplier, float deltaTime)
        {
            direction.y = 0;
            deltaPosition.y = 0;
            
            return direction.normalized * (deltaPosition.magnitude * multiplier / deltaTime);
        }
    }
}
