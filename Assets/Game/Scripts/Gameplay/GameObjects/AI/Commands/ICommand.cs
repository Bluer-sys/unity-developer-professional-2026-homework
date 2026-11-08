namespace SampleGame
{
	public interface ICommand
	{
		bool CanExecute();
		void Execute(StateMachine stateMachine);
	}
}
