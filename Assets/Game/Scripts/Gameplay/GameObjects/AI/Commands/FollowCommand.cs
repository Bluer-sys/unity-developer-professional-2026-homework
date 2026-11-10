using Modules.AI;
using UnityEngine;

namespace SampleGame
{
	public sealed class FollowCommand : StateMachineCommand
	{
		private readonly Blackboard _blackboard;
		private readonly CommandMarkerView _markers;
		private readonly UnitRadiusComponent _radiusComponent;
		private readonly GameObject _target;

		public FollowCommand(Blackboard blackboard, CommandMarkerView markers,
			UnitRadiusComponent radiusComponent, GameObject target)
			: base(StateName.Follow)
		{
			_blackboard = blackboard;
			_markers = markers;
			_radiusComponent = radiusComponent;
			_target = target;
		}

		public override bool CanExecute() =>
			_target != _blackboard.GetValue(BlackboardAPI.Character) && _target.IsAlive();

		protected override void Apply()
		{
			float distance = _radiusComponent.Value +
				_target.GetComponent<UnitRadiusComponent>().Value +
				_blackboard.GetValue(BlackboardAPI.StoppingDistance);

			_blackboard.SetReferenceValue(BlackboardAPI.Target, _target);
			_blackboard.SetPrimitiveValue(BlackboardAPI.FollowDistance, distance);
			_markers.ShowFollowMarker(_target.transform);
		}
	}
}
