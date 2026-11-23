using System;
using Unity.Entities;

namespace Game
{
	[Serializable]
	public struct AttackReadyRequest : IComponentData, IEnableableComponent
	{
		public Entity target;
	}
}
