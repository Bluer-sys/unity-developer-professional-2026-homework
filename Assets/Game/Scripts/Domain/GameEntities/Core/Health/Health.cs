using System;
using Unity.Entities;

namespace Game
{
	[Serializable]
	public struct Health : IComponentData
	{
		public int value;
	}
}
