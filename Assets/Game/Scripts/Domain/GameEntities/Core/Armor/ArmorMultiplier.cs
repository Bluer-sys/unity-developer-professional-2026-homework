using System;
using Unity.Entities;

namespace Game
{
	[Serializable]
	public struct ArmorMultiplier : IComponentData
	{
		public float value;
	}
}
