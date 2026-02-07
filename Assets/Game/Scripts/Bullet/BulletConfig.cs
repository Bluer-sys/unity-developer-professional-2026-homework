using UnityEngine;

namespace Game.Bullet
{
    [CreateAssetMenu(menuName = "Game/BulletConfig", fileName = "BulletConfig")]
    public class BulletConfig : ScriptableObject
    {
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
    }
}
