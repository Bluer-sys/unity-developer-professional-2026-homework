using System;
using Modules.AI;
using UnityEngine;

namespace SampleGame
{
	[Serializable]
	public sealed class CharacterBlackboardInstaller : IBlackboardInstaller
	{
		[SerializeField] private GameObject _character;
		[SerializeField] private float _attackDistance = 6.5f;
		[SerializeField] private float _stoppingDistance = 0.25f;
		[SerializeField] private int _colliderBufferSize = 32;

		public void Install(Blackboard blackboard)
		{
			blackboard.SetReferenceValue(BlackboardAPI.Character, _character);
			blackboard.SetPrimitiveValue(BlackboardAPI.Destination, _character.transform.position);
			blackboard.SetReferenceValue(BlackboardAPI.Waypoints, new Vector3[2]);
			blackboard.SetPrimitiveValue(BlackboardAPI.WaypointIndex, 0);
			blackboard.SetPrimitiveValue(BlackboardAPI.AttackDistance, _attackDistance);
			blackboard.SetPrimitiveValue(BlackboardAPI.StoppingDistance, _stoppingDistance);
			blackboard.SetPrimitiveValue(BlackboardAPI.FollowDistance, _stoppingDistance);
			blackboard.SetReferenceValue(BlackboardAPI.ColliderBuffer, new Collider[_colliderBufferSize]);
		}
	}
}
