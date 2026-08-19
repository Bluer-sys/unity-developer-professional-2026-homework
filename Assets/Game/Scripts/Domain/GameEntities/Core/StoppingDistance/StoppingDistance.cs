using System;
using Unity.Entities;

namespace Game
{
	[Serializable]
	public struct StoppingDistance : IComponentData
	{
		public float value;
	}
}
