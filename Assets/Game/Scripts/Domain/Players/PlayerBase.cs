using System;
using Unity.Entities;

namespace Game
{
	[Serializable]
	public struct PlayerBase : IComponentData
	{
		public Entity value;
	}
}
