using Modules.AI;
using Modules.FSM;
using UnityEngine;

namespace SampleGame
{
	public sealed class FireTargetState : State
	{
		[SerializeField] private Blackboard _blackboard;
		[SerializeField] private AttackComponent _attackComponent;
		[SerializeField] private string _targetKey = nameof(BlackboardAPI.Target);

		public override void OnEnter() {}

		public override void OnUpdate(float deltaTime)
		{
			if (!_blackboard.TryGetValue(_targetKey, out GameObject target) || !AIUtility.IsAlive(target))
				return;

			GameObject character = _blackboard.GetValue(BlackboardAPI.Character);
			Vector3 distance = AIUtility.GetDistanceVector(character.transform.position, target.transform.position);
			float attackDistance = _blackboard.GetValue(BlackboardAPI.AttackDistance);

			if (distance.sqrMagnitude <= attackDistance * attackDistance)
				_attackComponent.Attack(target);
		}

		public override void OnExit() {}
	}
}
