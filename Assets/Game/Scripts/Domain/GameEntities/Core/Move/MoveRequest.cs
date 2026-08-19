using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
	[Serializable]
	public struct MoveRequest : IComponentData, IEnableableComponent
	{
		public float3 direction;
	}
}
