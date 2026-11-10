using Modules.AI;
using UnityEngine;

namespace SampleGame
{
	public sealed class StopCommand : StateMachineCommand
	{
		private readonly Blackboard _blackboard;

		public StopCommand(Blackboard blackboard) : base(StateName.Idle)
		{
			_blackboard = blackboard;
		}

		protected override void Apply()
		{
			GameObject character = _blackboard.GetValue(BlackboardAPI.Character);
			_blackboard.SetPrimitiveValue(BlackboardAPI.Position, character.transform.position);
		}
	}
}
