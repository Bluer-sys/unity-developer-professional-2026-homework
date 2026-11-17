using System;
using Unity.Entities;

namespace Game
{
	[Serializable, InternalBufferCapacity(4)]
	public struct PurchaseUnitEvent : IBufferElementData
	{
		public CombatUnitType unitType;
		public Entity unit;
		public int price;
	}
}
