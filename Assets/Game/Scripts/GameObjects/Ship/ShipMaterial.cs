using UnityEngine;

namespace Game.GameObjects.Ship
{
    public class ShipMaterial : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        
        public Material Material { get; private set; }
        public Renderer Renderer => _renderer;

        public void SetMaterial(Material prefab)
        {
            Material = new Material(prefab);
            _renderer.material = Material;
        }
    }
}
