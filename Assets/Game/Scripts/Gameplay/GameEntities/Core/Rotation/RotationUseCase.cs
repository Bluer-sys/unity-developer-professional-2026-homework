using UnityEngine;

namespace Game.Gameplay
{
    public static class RotationUseCase
    {
        public static Quaternion GetRotation(Quaternion current, Vector3 direction, float speed, float deltaTime)
        {
            direction.y = 0;

            if (direction == Vector3.zero)
                return current;

            Quaternion target = Quaternion.LookRotation(direction);
            return Quaternion.RotateTowards(current, target, speed * deltaTime);
        }
    }
}
