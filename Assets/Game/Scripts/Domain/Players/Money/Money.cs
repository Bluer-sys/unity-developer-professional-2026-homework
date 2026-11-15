using System;
using Unity.Entities;

namespace Game
{
	[Serializable]
	public struct Money : IComponentData
	{
		public int value;
	}
}
