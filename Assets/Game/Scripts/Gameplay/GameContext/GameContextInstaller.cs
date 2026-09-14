using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class GameContextInstaller : MonoEntityInstaller<IGameContext>
    {
        [SerializeField] private PlayerContext _playerContext;

        public override void Install(IGameContext context)
        {
            context.AddPlayerContext(_playerContext);
        }
    }
}
