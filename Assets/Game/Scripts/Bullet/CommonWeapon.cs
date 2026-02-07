using Game.Common;
using Game.Data;

namespace Game.Bullet
{
    public sealed class CommonWeapon : IWeapon
    {
        private readonly BulletWorldGo _bulletWorld;
        private readonly ShipController _ship;
        private readonly TeamType _team;

        public CommonWeapon(BulletWorldGo bulletWorld, ShipController player, TeamType team)
        {
            _bulletWorld = bulletWorld;
            _ship = player;
            _team = team;
        }
        
        public void Fire()
        {
            _bulletWorld.Spawn(_ship.firePoint.position,
                _ship.firePoint.up,
                _ship.bulletSpeed,
                _ship.bulletDamage,
                _team);
        }
    }
}
