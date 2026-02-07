using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(menuName = "Game/ShipControllerInfo", fileName = "ShipControllerInfo")]
    public sealed class ShipControllerSo : ScriptableObject
    {
        [field: Header("Core")]
        [field: SerializeField]
        public int Health { get; private set; } = 5;

        [field: SerializeField]
        public float MoveSpeed { get; private set; } = 5;

        [field: SerializeField]
        public float FireCooldown { get; private set; } = 0.25f;
    }
}
