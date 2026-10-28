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
			Vector3 distance = AIUtility.GetDistanceVector(character.transform.position, destination);
			
			_moveComponent.MoveStep(distance.normalized, deltaTime);
		}

		public override void OnExit() {}
	}
}
