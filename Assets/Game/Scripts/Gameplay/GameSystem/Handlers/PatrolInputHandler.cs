using Modules.AI;
using UnityEngine;

namespace SampleGame
{
	public sealed class PatrolInputHandler : InputHandler
	{
		[SerializeField]
		private KeyCode _keyCode = KeyCode.P;

		[SerializeField]
		private SequencedStateMachine _stateMachine;

		[SerializeField]
		private Blackboard _blackboard;

		[SerializeField]
		private CommandMarkerView _markers;

		[SerializeField]
		private InputHandler _next;

		public override void Handle(ref InputContext context)
		{
			if (Input.GetKey(_keyCode) && context.leftClick)
			{
				if (context.point.HasValue)
				{
					Vector3 point = context.point.Value;
					if (context.enqueueCommand && _stateMachine.CurrentState == StateName.Patrol)
					{
						SetPatrolPoint(point);
						return;
					}

					_stateMachine.Submit(new StateMachineCommand(StateName.Patrol, () =>
					{
						Vector3[] waypoints = _blackboard.GetValue(BlackboardAPI.Waypoints);
						waypoints[0] = _blackboard.GetValue(BlackboardAPI.Character).transform.position;
						SetPatrolPoint(point);
					}), context.enqueueCommand);
				}
			}
			else if (_next)
				_next.Handle(ref context);
		}

		private void SetPatrolPoint(Vector3 point)
		{
			Vector3[] waypoints = _blackboard.GetValue(BlackboardAPI.Waypoints);
			waypoints[1] = point;
			_blackboard.SetPrimitiveValue(BlackboardAPI.WaypointIndex, 1);
			_markers.ShowPatrolMarker(point);
		}
	}
}
