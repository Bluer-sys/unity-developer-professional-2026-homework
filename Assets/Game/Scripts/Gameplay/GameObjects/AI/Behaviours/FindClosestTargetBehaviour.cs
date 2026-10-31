using Modules.AI;
using UnityEngine;

namespace SampleGame
{
	public sealed class FindClosestTargetBehaviour : MonoBehaviour
	{
		[SerializeField] private Blackboard _blackboard;
		[SerializeField] private TeamComponent _teamComponent;

		private void FixedUpdate()
		{
			GameObject character = _blackboard.GetValue(BlackboardAPI.Character);
			float radius = _blackboard.GetValue(BlackboardAPI.SightDistance);
			Collider[] buffer = _blackboard.GetValue(BlackboardAPI.ColliderBuffer);

			GameObject target = FindClosestTarget(character, radius, buffer);

			if (target)
				_blackboard.SetReferenceValue(BlackboardAPI.Target, target);
			else
				_blackboard.DelValue(BlackboardAPI.Target);
		}

		private GameObject FindClosestTarget(GameObject character, float radius, Collider[] buffer)
		{
			int count = Physics.OverlapSphereNonAlloc(character.transform.position,
				radius,
				buffer,
				Physics.AllLayers,
				QueryTriggerInteraction.Ignore);

			GameObject target = null;
			float minDistance = radius * radius;

			for (int i = 0; i < count; i++)
			{
				GameObject other = buffer[i].gameObject;

				if (other == character || 
					!AIUtility.IsAlive(other) || 
					!_teamComponent.IsEnemy(other))
					continue;

				Vector3 distance = AIUtility.GetDistanceV3(character.transform.position, other.transform.position);
				float sqrDistance = distance.sqrMagnitude;

				if (sqrDistance <= minDistance)
				{
					target = other;
					minDistance = sqrDistance;
				}
			}

			return target;
		}
	}
}
