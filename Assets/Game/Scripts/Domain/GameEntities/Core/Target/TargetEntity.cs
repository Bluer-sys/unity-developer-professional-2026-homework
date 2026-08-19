using System;
using Unity.Entities;

namespace Game
{
	[Serializable]
	public struct TargetEntity : IComponentData
	{
		public Entity value;
	}
}
