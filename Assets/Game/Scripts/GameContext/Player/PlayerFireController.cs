using Game.GameObjects;
using UnityEngine;

namespace Game.GameContext
{
    public class PlayerFireController : MonoBehaviour
    {
        [SerializeField] private Weapon _weapon;
        
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _weapon.TryFire(Vector2.up);
        }
    }
}
