using Unity.Entities;
using UnityEngine;

namespace Game
{
	[UpdateInGroup(typeof(PresentationSystemGroup))]
	public partial struct GameOverLogSystem : ISystem
	{
		public void OnUpdate(ref SystemState state)
		{
			foreach (RefRO<GameState> gameState in SystemAPI.Query<RefRO<GameState>>().WithAll<GameOverEvent>())
			{
				switch (gameState.ValueRO.value)
				{
					case GameStatus.Win:
						Debug.Log("Game Win");
						break;
					case GameStatus.Loss:
						Debug.Log("Game Over");
						break;
				}
			}
		}
	}
}
