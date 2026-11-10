using System.Collections.Generic;
using Modules.AI;
using UnityEngine;

namespace SampleGame
{
	public sealed class PatrolCommand : StateMachineCommand
	{
		private readonly Blackboard _blackboard;
		private readonly CommandMarkerView _markers;
		private readonly Vector3 _point;

		public PatrolCommand(Blackboard blackboard, CommandMarkerView markers, Vector3 point)
			: base(StateName.Patrol)
		{
			_blackboard = blackboard;
			_markers = markers;
			_point = point;
		}

		protected override void Apply()
		{
			List<Vector3> waypoints = _blackboard.GetValue(BlackboardAPI.Waypoints);
			Vector3 position = _blackboard.GetValue(BlackboardAPI.Character).transform.position;
			waypoints.Clear();
			waypoints.Add(position);
			waypoints.Add(_point);
			_markers.ShowPatrolMarker(_point);
			_blackboard.SetPrimitiveValue(BlackboardAPI.WaypointIndex, 1);
		}
	}
}
