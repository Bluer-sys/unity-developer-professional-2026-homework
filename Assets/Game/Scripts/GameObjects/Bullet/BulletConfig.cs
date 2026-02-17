using UnityEngine;

namespace Game.GameObjects.Bullet
{
    [CreateAssetMenu(menuName = "Game/BulletConfig", fileName = "BulletConfig")]
    public class BulletConfig : ScriptableObject
    {
        [field: SerializeField] private string LayerName { get; set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public bool IsRedVfx { get; private set; }
        
        public int GetLayer()
        {
            return LayerMask.NameToLayer(LayerName);
        }
    }
}
