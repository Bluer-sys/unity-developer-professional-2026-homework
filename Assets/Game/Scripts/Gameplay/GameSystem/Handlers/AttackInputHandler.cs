using Modules.AI;
using UnityEngine;

namespace SampleGame
{
	public sealed class AttackInputHandler : InputHandler
	{
		[SerializeField]
		private KeyCode _keyCode = KeyCode.A;

		[SerializeField]
		private SequencedStateMachine _stateMachine;

		[SerializeField]
		private Blackboard _blackboard;

		[SerializeField]
		private TeamComponent _teamComponent;

		[SerializeField]
		private CommandMarkerView _markers;

		[SerializeField]
		private InputHandler _next;

		public override void Handle(ref InputContext context)
		{
			if (Input.GetKey(_keyCode) && context.leftClick)
			{
				_stateMachine.Submit(new AttackCommand(_blackboard, _markers, _teamComponent, context.target),
					context.enqueueCommand);
			}
			else if (_next)
				_next.Handle(ref context);
		}
	}
}
