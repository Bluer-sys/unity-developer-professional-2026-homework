using Game.GameObjects.Weapon;
using UnityEngine;

namespace Game.GameContext.Player
{
    public class PlayerFireController : MonoBehaviour
    {
        [SerializeField] private CooldownWeapon _weapon;
        
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _weapon.TryFire(Vector2.up);
        }
    }
}
