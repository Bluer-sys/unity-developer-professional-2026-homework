using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
	[Serializable]
	public struct FireOffset : IComponentData
	{
		public float3 value;
	}
}
