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
					_stateMachine.Submit(new MoveCommand(_blackboard, _markers, context.point.Value),
						context.enqueueCommand);
				}
			}
			else if (_next)
				_next.Handle(ref context);
		}
	}
}
