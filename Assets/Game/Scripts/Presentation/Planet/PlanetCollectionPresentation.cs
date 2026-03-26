using System;
using System.Collections.Generic;
using Modules.Planets;
using Zenject;

namespace Game.Presentation
{
    public class PlanetCollectionPresentation : IInitializable, IDisposable
    {
        public IReadOnlyList<PlanetPresentation> PlanetPresentations => _planetPresentations;
        
        private readonly IReadOnlyList<IPlanet> _planets;
        private readonly IInstantiator _instantiator;
        private readonly List<PlanetPresentation> _planetPresentations;

        public PlanetCollectionPresentation(IReadOnlyList<IPlanet> planets, IInstantiator instantiator)
        {
            _planets = planets;
            _instantiator = instantiator;
            _planetPresentations = new List<PlanetPresentation>(_planets.Count);

            CreatePlanetPresentations();
        }

        public void Initialize()
        {
            foreach (var presentation in _planetPresentations)
                presentation.Initialize();
        }

        public void Dispose()
        {
            foreach (var presentation in _planetPresentations)
                presentation.Dispose();
        }

        private void CreatePlanetPresentations() 
        {
            foreach (IPlanet planet in _planets)
            {
                var presentation = _instantiator.Instantiate<PlanetPresentation>(new[] { planet });
                _planetPresentations.Add(presentation);
            }
        }
    }
}
