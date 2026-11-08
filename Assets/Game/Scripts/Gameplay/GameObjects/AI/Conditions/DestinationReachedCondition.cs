using System;
using Modules.AI;
using UnityEngine;

namespace SampleGame
{
	[Serializable]
	public sealed class DestinationReachedCondition : ICondition
	{
		[SerializeField] private Blackboard _blackboard;

		public bool Invoke()
		{
			GameObject character = _blackboard.GetValue(BlackboardAPI.Character);
			Vector3 destination = _blackboard.GetValue(BlackboardAPI.Destination);
			Vector3 distance = character.transform.position.GetDistanceXZ(destination);
			float stoppingDistance = _blackboard.GetValue(BlackboardAPI.StoppingDistance);
			
			return distance.sqrMagnitude <= stoppingDistance * stoppingDistance;
		}
	}
}
