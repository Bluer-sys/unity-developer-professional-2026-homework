using System;
using Modules.AI;
using UnityEngine;

namespace SampleGame
{
	[Serializable]
	public sealed class CharacterDeadCondition : ICondition
	{
		[SerializeField] private HealthComponent _healthComponent;

		public bool Invoke()
		{
			return _healthComponent.IsDead;
		}
	}
}
