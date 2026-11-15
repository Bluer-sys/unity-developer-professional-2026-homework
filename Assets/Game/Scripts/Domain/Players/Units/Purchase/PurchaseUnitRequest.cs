using System;
using Unity.Entities;

namespace Game
{
	[Serializable, InternalBufferCapacity(4)]
	public struct PurchaseUnitRequest : IBufferElementData
	{
		public CombatUnitType unitType;
	}
}
