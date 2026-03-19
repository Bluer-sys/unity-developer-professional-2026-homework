using Modules.Planets;

namespace Game.UI.Presentation.Provider
{
    public interface IPlanetsProvider
    {
        IPlanet NextPlanet();
    }
}
