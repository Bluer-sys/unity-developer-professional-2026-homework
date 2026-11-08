using Modules.AI;
using Modules.FSM;
using UnityEngine;

namespace SampleGame
{
	public sealed class FireTargetState : State
	{
		[SerializeField] private Blackboard _blackboard;
		[SerializeField] private AttackComponent _attackComponent;

		public override void OnEnter() {}

		public override void OnUpdate(float deltaTime)
		{
			if (!_blackboard.TryGetValue(BlackboardAPI.Target, out GameObject target) || !target.IsAlive())
				return;

			GameObject character = _blackboard.GetValue(BlackboardAPI.Character);
			Vector3 distance = character.transform.position.GetDistanceXZ(target.transform.position);
			float attackDistance = _blackboard.GetValue(BlackboardAPI.AttackDistance);

			if (distance.sqrMagnitude <= attackDistance * attackDistance)
				_attackComponent.Attack(target);
		}

		public override void OnExit() {}
	}
}
