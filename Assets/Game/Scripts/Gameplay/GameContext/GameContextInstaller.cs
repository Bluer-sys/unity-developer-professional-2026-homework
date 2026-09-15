using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class GameContextInstaller : MonoEntityInstaller<IGameContext>
    {
        [SerializeField] private PlayerContext _playerContext;
        [SerializeField] private BulletPool _bulletPool;

        public override void Install(IGameContext context)
        {
            context.AddPlayerContext(_playerContext);
            context.AddBulletPool(_bulletPool);
            context.AddKillCount(new ReactiveVariable<int>(0));
        }
    }
}
