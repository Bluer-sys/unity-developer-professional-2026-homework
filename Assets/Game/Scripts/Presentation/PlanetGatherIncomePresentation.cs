using Modules.Planets;
using R3;

namespace Game.Presentation
{
    public class PlanetGatherIncomePresentation
    {
        public Observable<IPlanet> OnGatherRequested => _onGatherRequested;
            
        private readonly ReactiveCommand<IPlanet> _onGatherRequested = new();
        
        public void BeginGather(IPlanet planet) =>
            _onGatherRequested.Execute(planet);

        public void EndGather(IPlanet planet) =>
            planet.GatherIncome();
    }
}
