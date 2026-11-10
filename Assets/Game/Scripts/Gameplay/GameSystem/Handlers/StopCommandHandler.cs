using Modules.AI;
using UnityEngine;

namespace SampleGame
{
	public sealed class StopCommandHandler : InputHandler
	{
		[SerializeField]
		private KeyCode _keyCode = KeyCode.S;

		[SerializeField]
		private SequencedStateMachine _stateMachine;

		[SerializeField]
		private Blackboard _blackboard;

		[SerializeField]
		private InputHandler _next;

		public override void Handle(ref InputContext context)
		{
			if (Input.GetKeyDown(_keyCode))
			{
				_stateMachine.Submit(new StopCommand(_blackboard), false);
			}
			else if (_next)
				_next.Handle(ref context);
		}
	}
}
