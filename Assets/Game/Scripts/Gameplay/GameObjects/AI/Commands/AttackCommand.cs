using Modules.AI;
using UnityEngine;

namespace SampleGame
{
	public sealed class AttackCommand : StateMachineCommand
	{
		private readonly Blackboard _blackboard;
		private readonly CommandMarkerView _markers;
		private readonly TeamComponent _teamComponent;
		private readonly GameObject _target;

		public AttackCommand(Blackboard blackboard, CommandMarkerView markers,
			TeamComponent teamComponent, GameObject target)
			: base(StateName.Attack)
		{
			_blackboard = blackboard;
			_markers = markers;
			_teamComponent = teamComponent;
			_target = target;
		}

		public override bool CanExecute() => _target.IsAlive() && _teamComponent.IsEnemy(_target);

		protected override void Apply()
		{
			_blackboard.SetReferenceValue(BlackboardAPI.Target, _target);
			_blackboard.SetPrimitiveValue(BlackboardAPI.ChaseDistance, float.PositiveInfinity);
			_markers.ShowAttackMarker(_target.transform);
		}
	}
}
