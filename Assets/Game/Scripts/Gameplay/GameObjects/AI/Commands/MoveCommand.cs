using Modules.AI;
using UnityEngine;

namespace SampleGame
{
	public sealed class MoveCommand : StateMachineCommand
	{
		private readonly Blackboard _blackboard;
		private readonly CommandMarkerView _markers;
		private readonly Vector3 _point;

		public MoveCommand(Blackboard blackboard, CommandMarkerView markers, Vector3 point)
			: base(StateName.Move)
		{
			_blackboard = blackboard;
			_markers = markers;
			_point = point;
		}

		protected override void Apply()
		{
			_blackboard.SetPrimitiveValue(BlackboardAPI.Destination, _point);
			_markers.ShowMoveMarker(_point);
		}
	}
}
