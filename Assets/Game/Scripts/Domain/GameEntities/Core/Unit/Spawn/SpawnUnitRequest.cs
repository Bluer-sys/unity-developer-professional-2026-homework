using System;
using Unity.Entities;

namespace Game
{
	[Serializable, InternalBufferCapacity(4)]
	public struct SpawnUnitRequest : IBufferElementData
	{
		public UnitType unitType;
		public Entity prefab;
		public int price;
	}
}
