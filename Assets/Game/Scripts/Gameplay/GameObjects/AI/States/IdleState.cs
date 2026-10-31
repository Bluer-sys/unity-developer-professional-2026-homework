using Modules.AI;
using Modules.FSM;
using UnityEngine;

namespace SampleGame
{
	public sealed class IdleState : State
	{
		[SerializeField] private Blackboard _blackboard;

		public override void OnEnter()
		{
			_blackboard.DelValue(BlackboardAPI.Target);
			_blackboard.SetPrimitiveValue(BlackboardAPI.ChaseDistance, _blackboard.GetValue(BlackboardAPI.SightDistance));
			_blackboard.SetPrimitiveValue(BlackboardAPI.Destination, _blackboard.GetValue(BlackboardAPI.Position));
		}

		public override void OnUpdate(float deltaTime) {}

		public override void OnExit() {}
	}
}
