using System;
using Unity.Entities;

namespace Game
{
	[Serializable]
	public struct GameState : IComponentData
	{
		public GameStatus value;

		public bool IsRunning => value == GameStatus.Running;
	}
}
