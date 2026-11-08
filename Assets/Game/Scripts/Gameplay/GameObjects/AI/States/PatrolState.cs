using Modules.AI;
using Modules.FSM;
using UnityEngine;

namespace SampleGame
{
	public sealed class PatrolState : State
	{
		[SerializeField] private Blackboard _blackboard;
		[SerializeField] private MoveComponent _moveComponent;

		public override void OnEnter() {}

		public override void OnUpdate(float deltaTime)
		{
			GameObject character = _blackboard.GetValue(BlackboardAPI.Character);
			Vector3[] waypoints = _blackboard.GetValue(BlackboardAPI.Waypoints);
			int index = _blackboard.GetValue(BlackboardAPI.WaypointIndex);
			float stoppingDistance = _blackboard.GetValue(BlackboardAPI.StoppingDistance);
			Vector3 distance = character.transform.position.GetDistanceXZ(waypoints[index]);

			if (distance.sqrMagnitude <= stoppingDistance * stoppingDistance)
				_blackboard.SetPrimitiveValue(BlackboardAPI.WaypointIndex, (index + 1) % waypoints.Length);
			else
				_moveComponent.MoveStep(distance.normalized, deltaTime);
		}

		public override void OnExit() {}
	}
}
