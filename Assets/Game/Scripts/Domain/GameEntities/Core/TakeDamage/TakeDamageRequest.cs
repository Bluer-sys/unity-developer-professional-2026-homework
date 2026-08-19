using Unity.Entities;

namespace Game
{
	[InternalBufferCapacity(4)]
	public struct TakeDamageRequest : IBufferElementData
	{
		public int damage;
		public Entity instigator;
	}
}
