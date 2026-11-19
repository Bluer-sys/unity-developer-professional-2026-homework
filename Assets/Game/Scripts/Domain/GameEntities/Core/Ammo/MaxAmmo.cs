using System;
using Unity.Entities;

namespace Game
{
	[Serializable]
	public struct MaxAmmo : IComponentData
	{
		public int value;
	}
}
