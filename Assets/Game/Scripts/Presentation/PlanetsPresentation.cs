using System.Collections.Generic;
using Modules.Planets;

namespace Game.Presentation
{
    public class PlanetsPresentation
    {
        public IReadOnlyList<IPlanet> Planets { get; }

        public PlanetsPresentation(IReadOnlyList<IPlanet> planets)
        {
            Planets = planets;
        }
    }
}
