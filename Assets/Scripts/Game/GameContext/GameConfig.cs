// ReSharper disable Unity.RedundantSerializeFieldAttribute
// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.GameContext
{
    [CreateAssetMenu(menuName = "Configs/GameConfig", fileName = "GameConfig")]
    public class GameConfig : SerializedScriptableObject
    {
        [field: SerializeField] public int MaxDifficulty { get; private set; }
        [field: SerializeField] public Dictionary<int, float> SnakeDifficultiesSpeed { get; private set; }

        public float GetSnakeSpeed(int difficulty)
        {
            return SnakeDifficultiesSpeed[difficulty];
        }
    }
}
