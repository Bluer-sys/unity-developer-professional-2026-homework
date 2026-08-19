using System;
using Unity.Entities;

namespace Game
{
	[Serializable]
	public struct FireCooldown : IComponentData
	{
		public float time;
		public float duration;
	}
}
