using Modules.AI;
using Modules.FSM;
using UnityEngine;

namespace SampleGame
{
	public sealed class FollowTargetState : State
	{
		[SerializeField] private Blackboard _blackboard;
		[SerializeField] private MoveComponent _moveComponent;
		[SerializeField] private string _distanceKey = nameof(BlackboardAPI.FollowDistance);

		public override void OnEnter() {}

		public override void OnUpdate(float deltaTime)
		{
			GameObject character = _blackboard.GetValue(BlackboardAPI.Character);
			GameObject target = _blackboard.GetValue(BlackboardAPI.Target);
			Vector3 distance = character.transform.position.GetDistanceXZ(target.transform.position);
			float stoppingDistance = _blackboard.GetValue<float>(_distanceKey);

			if (distance.sqrMagnitude > stoppingDistance * stoppingDistance)
				_moveComponent.MoveStep(distance.normalized, deltaTime);
		}

		public override void OnExit() {}
	}
}
