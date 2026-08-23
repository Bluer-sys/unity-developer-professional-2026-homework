using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "InputMap", menuName = "Game/System/New InputMap")]
    public sealed class InputMap : ScriptableObject
    {
        public Vector2 GetMoveDirection() => 
            new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
}
