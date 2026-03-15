using System.Collections.Generic;
using Modules.Planets;
using R3;

namespace Game.Presentation
{
    public class PlanetGatherIncomePresentation
    {
        public Observable<IPlanet> OnGatherRequested => _onGatherRequested;
            
        private readonly ReactiveCommand<IPlanet> _onGatherRequested = new();

        private readonly HashSet<IPlanet> _processingPlanets = new();
        
        public void BeginGather(IPlanet planet)
        {
            if(_processingPlanets.Contains(planet))
                return;
            
            _onGatherRequested.Execute(planet);
            _processingPlanets.Add(planet);
        }

        public void EndGather(IPlanet planet)
        {
            planet.GatherIncome();
            _processingPlanets.Remove(planet);
        }
    }
}
