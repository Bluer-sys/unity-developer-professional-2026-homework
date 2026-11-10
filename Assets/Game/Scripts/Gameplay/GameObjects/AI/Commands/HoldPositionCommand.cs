using Modules.AI;

namespace SampleGame
{
	public sealed class HoldPositionCommand : StateMachineCommand
	{
		private readonly Blackboard _blackboard;
		private readonly CommandMarkerView _markers;

		public HoldPositionCommand(Blackboard blackboard, CommandMarkerView markers)
			: base(StateName.HoldPosition)
		{
			_blackboard = blackboard;
			_markers = markers;
		}

		protected override void Apply()
		{
			_blackboard.DelValue(BlackboardAPI.Target);
			_markers.ShowHoldPositionMarker(_blackboard.GetValue(BlackboardAPI.Character).transform.position);
		}
	}
}
