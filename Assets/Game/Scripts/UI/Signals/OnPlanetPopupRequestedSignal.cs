using Modules.Planets;

namespace Game.UI
{
    public readonly struct OnPlanetPopupRequestedSignal
    {
        public readonly IPlanet Planet;

        public OnPlanetPopupRequestedSignal(IPlanet planet)
        {
            Planet = planet;
        }
    }
}
