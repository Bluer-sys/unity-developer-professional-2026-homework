using System.Collections.Generic;
using Modules.FSM;
using UnityEngine;

namespace SampleGame
{
	public sealed class SequencedStateMachine : State
	{
		[SerializeField] private StateMachine _stateMachine;

		private readonly Queue<ICommand> _commands = new();
		private ICommand _currentCommand;

		public StateName CurrentState => _stateMachine.CurrentState;

		public override void OnEnter()
		{
			_stateMachine.OnEnter();
		}

		public override void OnUpdate(float deltaTime)
		{
			_stateMachine.OnUpdate(deltaTime);

			if (CurrentState == StateName.Idle)
				_currentCommand = null;
			
			if (_currentCommand == null)
				ExecuteNext();
		}

		public override void OnExit()
		{
			_stateMachine.OnExit();
			_commands.Clear();
			_currentCommand = null;
		}

		public void Submit(ICommand command, bool enqueue)
		{
			if (!isActiveAndEnabled || !command.CanExecute())
				return;

			if (enqueue && _currentCommand != null)
			{
				_commands.Enqueue(command);
				return;
			}

			if (!enqueue)
				_commands.Clear();

			Execute(command);
		}

		private void ExecuteNext()
		{
			while (_commands.Count > 0)
			{
				ICommand command = _commands.Dequeue();
				if (!command.CanExecute())
					continue;

				Execute(command);
				return;
			}
		}

		private void Execute(ICommand command)
		{
			_currentCommand = command;
			command.Execute(_stateMachine);
		}
	}
}
