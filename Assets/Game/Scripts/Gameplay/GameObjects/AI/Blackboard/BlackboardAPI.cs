using Modules.AI;
using UnityEngine;

namespace SampleGame
{
	[BlackboardAPI]
	public static class BlackboardAPI
	{
		public static readonly BlackboardValueKey<GameObject> Character = new(nameof(Character));
		public static readonly BlackboardValueKey<GameObject> Target = new(nameof(Target));
		public static readonly BlackboardValueKey<Vector3> Destination = new(nameof(Destination));
		public static readonly BlackboardValueKey<Vector3> Position = new(nameof(Position));
		
		public static readonly BlackboardValueKey<Vector3[]> Waypoints = new(nameof(Waypoints));
		public static readonly BlackboardValueKey<int> WaypointIndex = new(nameof(WaypointIndex));
		
		public static readonly BlackboardValueKey<float> AttackDistance = new(nameof(AttackDistance));
		public static readonly BlackboardValueKey<float> SightDistance = new(nameof(SightDistance));
		public static readonly BlackboardValueKey<float> ChaseDistance = new(nameof(ChaseDistance));
		public static readonly BlackboardValueKey<float> StoppingDistance = new(nameof(StoppingDistance));
		public static readonly BlackboardValueKey<float> FollowDistance = new(nameof(FollowDistance));
		public static readonly BlackboardValueKey<Collider[]> ColliderBuffer = new(nameof(ColliderBuffer));
	}
}
