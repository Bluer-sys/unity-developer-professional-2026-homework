using Modules.AI;
using UnityEngine;

namespace SampleGame
{
	public sealed class HoldPositionHandler : InputHandler
	{
		[SerializeField]
		private KeyCode _keyCode = KeyCode.H;

		[SerializeField]
		private StateMachine _stateMachine;

		[SerializeField]
		private Blackboard _blackboard;

		[SerializeField]
		private CommandMarkerView _markers;

		[SerializeField]
		private InputHandler _next;

		public override void Handle(ref InputContext context)
		{
			if (Input.GetKeyDown(_keyCode))
			{
				_stateMachine.ChangeState(StateName.HoldPosition);
				_markers.ShowHoldPositionMarker(_blackboard.GetValue(BlackboardAPI.Character).transform.position);
			}
			else if (_next)
				_next.Handle(ref context);
		}
	}
}
