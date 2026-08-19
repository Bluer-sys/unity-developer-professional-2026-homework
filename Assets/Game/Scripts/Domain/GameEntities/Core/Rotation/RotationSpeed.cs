using System;
using Unity.Entities;

namespace Game
{
	[Serializable]
	public struct RotationSpeed : IComponentData
	{
		public float value;
	}
}
