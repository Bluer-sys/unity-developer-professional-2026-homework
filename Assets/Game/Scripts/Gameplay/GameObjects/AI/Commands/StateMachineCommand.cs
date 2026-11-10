namespace SampleGame
{
	public abstract class StateMachineCommand : ICommand
	{
		private readonly StateName _state;

		protected StateMachineCommand(StateName state)
		{
			_state = state;
		}

		public virtual bool CanExecute() => true;

		public void Execute(StateMachine stateMachine) => stateMachine.ChangeState(_state, Apply);

		protected abstract void Apply();
	}
}
