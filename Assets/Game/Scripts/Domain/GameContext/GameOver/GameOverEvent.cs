using System;
using Unity.Entities;

namespace Game
{
	[Serializable]
	public struct GameOverEvent : IComponentData, IEnableableComponent {}
}
