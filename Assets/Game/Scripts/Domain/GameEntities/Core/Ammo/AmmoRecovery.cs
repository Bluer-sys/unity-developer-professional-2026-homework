using System;
using Unity.Entities;

namespace Game
{
	[Serializable]
	public struct AmmoRecovery : IComponentData
	{
		public float time;
		public float duration;
	}
}
