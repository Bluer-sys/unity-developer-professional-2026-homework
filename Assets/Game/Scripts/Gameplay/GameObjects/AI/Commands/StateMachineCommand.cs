using System;

namespace SampleGame
{
	public sealed class StateMachineCommand : ICommand
	{
		private readonly StateName _state;
		private readonly Action _action;
		private readonly Func<bool> _condition;

		public StateMachineCommand(StateName state, Action action, Func<bool> condition = null)
		{
			_state = state;
			_action = action;
			_condition = condition;
		}

		public bool CanExecute() => _condition == null || _condition();

		public void Execute(StateMachine stateMachine) => stateMachine.ChangeState(_state, _action);
	}
}
