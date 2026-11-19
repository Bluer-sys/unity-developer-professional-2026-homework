using System;
using Unity.Entities;

namespace Game
{
	[Serializable]
	public struct DetectionCooldown : IComponentData
	{
		public float time;
		public float duration;
	}
}
