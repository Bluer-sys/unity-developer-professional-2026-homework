using System;
using Unity.Entities;

namespace Game
{
	[Serializable]
	public struct MoveSpeed : IComponentData
	{
		public float value;
	}
}
