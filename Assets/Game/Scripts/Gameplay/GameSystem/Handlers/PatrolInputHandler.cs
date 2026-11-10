using System.Collections.Generic;
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
						AddPatrolPoint(point);
						return;
					}

					_stateMachine.Submit(new PatrolCommand(_blackboard, _markers, point),
						context.enqueueCommand);
				}
			}
			else if (_next)
				_next.Handle(ref context);
		}

		private void AddPatrolPoint(Vector3 point)
		{
			List<Vector3> waypoints = _blackboard.GetValue(BlackboardAPI.Waypoints);
			waypoints.Add(point);
			_markers.ShowPatrolMarker(point);
		}
	}
}
