using UnityEngine;

namespace Game.Common
{
    public class AttackComponent : MonoBehaviour
    {
        private IWeapon _weapon;

        public void Construct(IWeapon weapon)
        {
            _weapon = weapon;
        }
        
        public void Fire()
        {
            _weapon.Fire();
        }
    }
}
