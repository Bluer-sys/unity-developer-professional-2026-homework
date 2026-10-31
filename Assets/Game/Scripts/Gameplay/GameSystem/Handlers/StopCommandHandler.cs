using Modules.AI;
using UnityEngine;

namespace SampleGame
{
	public sealed class StopCommandHandler : InputHandler
	{
		[SerializeField]
		private KeyCode _keyCode = KeyCode.S;

		[SerializeField]
		private StateMachine _stateMachine;

		[SerializeField]
		private Blackboard _blackboard;

		[SerializeField]
		private InputHandler _next;

		public override void Handle(ref InputContext context)
		{
			if (Input.GetKeyDown(_keyCode))
			{
				_stateMachine.ChangeState(StateName.Idle, () =>
				{
					GameObject character = _blackboard.GetValue(BlackboardAPI.Character);
					_blackboard.SetPrimitiveValue(BlackboardAPI.Position, character.transform.position);
				});
			}
			else if (_next)
				_next.Handle(ref context);
		}
	}
}
