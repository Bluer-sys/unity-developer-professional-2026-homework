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
				GameObject target = context.target;

				_stateMachine.Submit(new StateMachineCommand(StateName.Follow, () =>
				{
					float distance = _radiusComponent.Value + 
									 target.GetComponent<UnitRadiusComponent>().Value + 
									 _blackboard.GetValue(BlackboardAPI.StoppingDistance);
						
					_blackboard.SetReferenceValue(BlackboardAPI.Target, target);
					_blackboard.SetPrimitiveValue(BlackboardAPI.FollowDistance, distance);
					_markers.ShowFollowMarker(target.transform);
				}, () => target != _blackboard.GetValue(BlackboardAPI.Character) && AIUtility.IsAlive(target)),
					context.enqueueCommand);
			}
			else if (_next)
				_next.Handle(ref context);
		}
	}
}
