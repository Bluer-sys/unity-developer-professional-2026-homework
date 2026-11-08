using UnityEngine;

namespace SampleGame
{
	public static class AIExtensions
	{
		public static bool IsAlive(this GameObject target)
		{
			return target &&
				   target.activeInHierarchy &&
				   target.TryGetComponent(out HealthComponent health) &&
				   health.IsAlive;
		}

		public static Vector3 GetDistanceXZ(this Vector3 from, Vector3 to)
		{
			Vector3 distance = to - from;
			distance.y = 0;
			return distance;
		}
	}
}
