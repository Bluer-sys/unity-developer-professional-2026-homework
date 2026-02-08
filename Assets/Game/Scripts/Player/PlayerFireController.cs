using Game.Interfaces;
using UnityEngine;

namespace Game.Player
{
    public class PlayerFireController : MonoBehaviour
    {
        private IWeapon _weapon;

        public void Construct(IWeapon weapon)
        {
            _weapon = weapon;
        }
        
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _weapon.TryFire(Vector2.up);
        }
    }
}
