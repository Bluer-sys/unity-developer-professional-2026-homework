using System;
using System.Collections.Generic;
using Modules.Planets;
using Zenject;

namespace Game.Presentation
{
    public class PlanetCollectionPresentation : IInitializable, IDisposable
    {
        public IReadOnlyList<PlanetPresenter> PlanetPresentations => _planetPresentations;
        
        private readonly IReadOnlyList<IPlanet> _planets;
        private readonly IInstantiator _instantiator;
        private readonly List<PlanetPresenter> _planetPresentations;

        public PlanetCollectionPresentation(IReadOnlyList<IPlanet> planets, IInstantiator instantiator)
        {
            _planets = planets;
            _instantiator = instantiator;
            _planetPresentations = new List<PlanetPresenter>(_planets.Count);

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
                var presentation = _instantiator.Instantiate<PlanetPresenter>(new[] { planet });
                _planetPresentations.Add(presentation);
            }
        }
    }
}
