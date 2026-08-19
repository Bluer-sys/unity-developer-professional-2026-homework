using System;
using Unity.Entities;

namespace Game
{
	[Serializable]
	public struct Ammo : IComponentData
	{
		public int value;
	}
}
