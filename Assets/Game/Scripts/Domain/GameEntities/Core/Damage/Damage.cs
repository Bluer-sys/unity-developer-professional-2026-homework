using System;
using Unity.Entities;

namespace Game
{
	[Serializable]
	public struct Damage : IComponentData
	{
		public int value;
	}
}
