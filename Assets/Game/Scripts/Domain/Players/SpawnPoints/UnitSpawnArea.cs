using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
	[Serializable]
	public struct UnitSpawnArea : IComponentData
	{
		public float3 offset;
		public float radius;
	}
}
