using System;
using Modules.AI;
using UnityEngine;

namespace SampleGame
{
	[Serializable]
	public sealed class TargetDistanceCondition : ICondition
	{
		[SerializeField] private Blackboard _blackboard;

		[SerializeField]
		private string _distanceKey = nameof(BlackboardAPI.AttackDistance);

		public bool Invoke()
		{
			if (!_blackboard.TryGetValue(BlackboardAPI.Target, out GameObject target) || !AIUtility.IsAlive(target))
				return false;

			GameObject character = _blackboard.GetValue(BlackboardAPI.Character);
			Vector3 distance = AIUtility.GetDistanceVector(character.transform.position, target.transform.position);
			float targetDistance = _blackboard.GetValue<float>(_distanceKey);
			
			return distance.sqrMagnitude <= targetDistance * targetDistance;
		}
	}
}
