using System;
using Unity.Entities;

namespace Game
{
	[Serializable, InternalBufferCapacity(2)]
	public struct UnitCatalogItem : IBufferElementData
	{
		public UnitType unitType;
		public Entity prefab;
		public int price;
	}
}
