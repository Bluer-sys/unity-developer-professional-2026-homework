using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "InputMap", menuName = "Game/System/New InputMap")]
    public sealed class InputMap : ScriptableObject
    {
        [SerializeField] private KeyCode _turretKey = KeyCode.E;
        [SerializeField] private KeyCode _mineKey = KeyCode.Q;
        
        public Vector2 GetMoveDirection() => new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
        public bool IsTurretPressed() => Input.GetKeyDown(_turretKey);
        public bool IsMinePressed() => Input.GetKeyDown(_mineKey);
    }
}
