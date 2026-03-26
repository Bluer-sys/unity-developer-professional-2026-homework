using Modules.Planets;

namespace Game.Presentation
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
