using System;
using Unity.Entities;

namespace Game
{
	[Serializable]
	public struct Lifetime : IComponentData
	{
		public float value;
	}
}
