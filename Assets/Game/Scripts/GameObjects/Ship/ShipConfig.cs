using UnityEngine;

namespace Game.GameObjects.Ship
{
    [CreateAssetMenu(menuName = "Game/ShipControllerInfo", fileName = "ShipControllerInfo")]
    public sealed class ShipConfig : ScriptableObject
    {
        [field: Header("Core")]
        [field: SerializeField] public int Health { get; private set; } = 5;
        [field: SerializeField] public float MoveSpeed { get; private set; } = 5;
        [field: SerializeField] public float FireCooldown { get; private set; } = 0.25f;
        [field: SerializeField] public float StoppingDistance { get; private set; }
    }
}
