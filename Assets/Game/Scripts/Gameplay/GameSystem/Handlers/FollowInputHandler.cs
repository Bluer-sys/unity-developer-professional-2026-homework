using Modules.AI;
using UnityEngine;

namespace SampleGame
{
	public sealed class FollowInputHandler : InputHandler
	{
		[SerializeField]
		private KeyCode _keyCode = KeyCode.F;

		[SerializeField]
		private SequencedStateMachine _stateMachine;

		[SerializeField]
		private Blackboard _blackboard;

		[SerializeField]
		private UnitRadiusComponent _radiusComponent;

		[SerializeField]
		private CommandMarkerView _markers;

		[SerializeField]
		private InputHandler _next;

		public override void Handle(ref InputContext context)
		{
			if (Input.GetKey(_keyCode) && context.leftClick)
			{
				_stateMachine.Submit(new FollowCommand(_blackboard, _markers, _radiusComponent, context.target),
					context.enqueueCommand);
			}
			else if (_next)
				_next.Handle(ref context);
		}
	}
}
