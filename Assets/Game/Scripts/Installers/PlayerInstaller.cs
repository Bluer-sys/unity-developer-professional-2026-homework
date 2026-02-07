using Game.Bullet;
using Game.Common;
using Game.Data;
using UnityEngine;

namespace Game.Installers
{
    public class PlayerInstaller : MonoBehaviour
    {
        [SerializeField] private AttackComponent _attack;
        [SerializeField] private BulletWorldGo _bulletWorld;
        [SerializeField] private ShipController _ship;
        
        
        private void Awake()
        {
            var weapon = new CommonWeapon(_bulletWorld, _ship, TeamType.Player);
            
            _attack.Construct(weapon);
        }
    }
}
