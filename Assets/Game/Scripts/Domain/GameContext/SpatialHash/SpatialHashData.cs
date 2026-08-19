using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;

namespace Game
{
	// Singleton
	public unsafe struct SpatialHashData : IComponentData //?
	{
		public UnsafeParallelMultiHashMap<uint, Entity>* map;
		public float cellSize;
	}
}
