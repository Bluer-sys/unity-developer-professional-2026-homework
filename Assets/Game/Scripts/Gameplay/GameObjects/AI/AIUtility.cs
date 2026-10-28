using UnityEngine;

namespace SampleGame
{
    public static class AIUtility
    {
        public static bool IsAlive(GameObject target)
        {
            return target && 
				   target.activeInHierarchy &&
                   target.TryGetComponent(out HealthComponent health) && 
				   health.IsAlive;
        }

        public static Vector3 GetDistanceVector(Vector3 from, Vector3 to)
        {
            Vector3 distance = to - from;
            distance.y = 0;
            return distance;
        }
    }
}
