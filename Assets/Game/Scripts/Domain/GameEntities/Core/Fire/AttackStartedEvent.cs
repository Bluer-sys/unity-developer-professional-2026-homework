using System;
using Unity.Entities;

namespace Game
{
	[Serializable]
	public struct AttackStartedEvent : IComponentData, IEnableableComponent {}
}
