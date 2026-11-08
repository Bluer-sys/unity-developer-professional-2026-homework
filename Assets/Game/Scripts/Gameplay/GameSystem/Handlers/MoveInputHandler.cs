using Modules.AI;
using UnityEngine;

namespace SampleGame
{
	public sealed class MoveInputHandler : InputHandler
	{
		[SerializeField]
		private KeyCode _keyCode = KeyCode.M;

		[SerializeField]
		private SequencedStateMachine _stateMachine;

		[SerializeField]
		private Blackboard _blackboard;

		[SerializeField]
		private CommandMarkerView _markers;

		[SerializeField]
		private InputHandler _next;

		public override void Handle(ref InputContext context)
		{
			if (Input.GetKey(_keyCode) && context.leftClick)
			{
				if (context.point.HasValue)
				{
					Vector3 point = context.point.Value;
					_stateMachine.Submit(new StateMachineCommand(StateName.Move, () =>
					{
						_blackboard.SetPrimitiveValue(BlackboardAPI.Destination, point);
						_markers.ShowMoveMarker(point);
					}), context.enqueueCommand);
				}
			}
			else if (_next)
				_next.Handle(ref context);
		}
	}
}
