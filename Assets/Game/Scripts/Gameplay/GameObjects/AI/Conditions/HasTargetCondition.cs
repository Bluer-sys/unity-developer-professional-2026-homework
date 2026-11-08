using System;
using Modules.AI;
using UnityEngine;

namespace SampleGame
{
	[Serializable]
	public sealed class HasTargetCondition : ICondition
	{
		[SerializeField] private Blackboard _blackboard;

		public bool Invoke()
		{
			return _blackboard.TryGetValue(BlackboardAPI.Target, out GameObject target) && target.IsAlive();
		}
	}
}
