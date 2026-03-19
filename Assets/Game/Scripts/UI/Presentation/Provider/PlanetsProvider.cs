using System.Collections.Generic;
using Modules.Planets;

namespace Game.UI.Presentation.Provider
{
    public class PlanetsProvider : IPlanetsProvider
    {
        private readonly IReadOnlyList<IPlanet> _planets;
        private int _curIndex;

        public PlanetsProvider(IReadOnlyList<IPlanet> planets) =>
            _planets = planets;

        public IPlanet NextPlanet()
        {
            if (_curIndex >= _planets.Count)
                return null;

            IPlanet planet = _planets[_curIndex];
            _curIndex++;
            return planet;
        }
    }
}
