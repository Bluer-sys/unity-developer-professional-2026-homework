using Modules.AI;
using Modules.FSM;
using UnityEngine;

namespace SampleGame
{
	public sealed class MoveToPointState : State
	{
		[SerializeField] private Blackboard _blackboard;
		[SerializeField] private MoveComponent _moveComponent;

		public override void OnEnter() {}

		public override void OnUpdate(float deltaTime)
		{
			GameObject character = _blackboard.GetValue(BlackboardAPI.Character);
			Vector3 destination = _blackboard.GetValue(BlackboardAPI.Destination);
			Vector3 distance = character.transform.position.GetDistanceXZ(destination);
			
			_moveComponent.MoveStep(distance.normalized, deltaTime);
		}

		public override void OnExit()
		{
			GameObject character = _blackboard.GetValue(BlackboardAPI.Character);
			Vector3 destination = _blackboard.GetValue(BlackboardAPI.Destination);
			Vector3 distance = character.transform.position.GetDistanceXZ(destination);
			float stoppingDistance = _blackboard.GetValue(BlackboardAPI.StoppingDistance);

			if (distance.sqrMagnitude <= stoppingDistance * stoppingDistance)
				_blackboard.SetPrimitiveValue(BlackboardAPI.Position, destination);
		}
	}
}
