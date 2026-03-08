using UnityEngine;

namespace Game.GameContext
{
    [CreateAssetMenu(menuName = "Configs/GameConfig", fileName = "GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public int MaxDifficulty { get; private set; }
    }
}
