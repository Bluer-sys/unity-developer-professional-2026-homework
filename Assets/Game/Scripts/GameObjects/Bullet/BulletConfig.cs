using UnityEngine;

namespace Game.GameObjects
{
    [CreateAssetMenu(menuName = "Game/BulletConfig", fileName = "BulletConfig")]
    public class BulletConfig : ScriptableObject
    {
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }

        [field: SerializeField] public bool IsRedVfx { get; private set; }
        [field: SerializeField] public GameObject ExplosionPrefab { get; private set; }
        
        [SerializeField] private string _layerName;

        public int GetLayer()
        {
            return LayerMask.NameToLayer(_layerName);
        }
    }
}
