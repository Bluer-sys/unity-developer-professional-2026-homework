using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class PlayerContextInstaller : MonoEntityInstaller<IPlayerContext>
    {
        [SerializeField] private GameEntity _character;

        public override void Install(IPlayerContext context)
        {
            context.AddCharacter(_character);
            context.AddBehaviour(new PlayerInputBehaviour());
        }
    }
}
