using System;
using System.Collections.Generic;
using Modules.Planets;
using R3;
using Zenject;

namespace Game.Presentation.Planet
{
    public class PlanetsPresentation : IInitializable, IDisposable
    {
        public Observable<IReadOnlyList<PlanetPresentation>> OnPresentersCreated => _onPresentersCreated;
        
        private readonly ReactiveCommand<IReadOnlyList<PlanetPresentation>> _onPresentersCreated = new();
        
        private readonly IReadOnlyList<IPlanet> _planets;
        private readonly IInstantiator _instantiator;
        private readonly List<PlanetPresentation> _presentations;

        public PlanetsPresentation(IReadOnlyList<IPlanet> planets, IInstantiator instantiator)
        {
            _planets = planets;
            _instantiator = instantiator;
            _presentations = new List<PlanetPresentation>(_planets.Count);
        }

        public void Initialize() => CreatePresenters();

        public void Dispose()
        {
            foreach (var presentation in _presentations)
                presentation.Dispose();
        }

        private void CreatePresenters()
        {
            foreach (IPlanet planet in _planets)
            {
                var presentation = _instantiator.Instantiate<PlanetPresentation>(new[] { planet });
                presentation.Initialize();

                _presentations.Add(presentation);
            }

            _onPresentersCreated?.Execute(_presentations);
        }
    }
}
