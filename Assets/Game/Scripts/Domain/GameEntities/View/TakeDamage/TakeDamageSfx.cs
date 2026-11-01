using System;
using Unity.Collections;
using Unity.Entities;

namespace Game
{
	[Serializable]
	public struct TakeDamageSfx : IComponentData
	{
		public FixedString64Bytes value;
	}
}
