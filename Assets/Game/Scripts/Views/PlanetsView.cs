using System.Collections.Generic;
using Game.Presentation;
using UnityEngine;
using Zenject;

namespace Game.Views
{
    public class PlanetsView : MonoBehaviour
    {
        [SerializeField] private PlanetView[] _planetsViews;

        private PlanetsPresentation _presentation;
        private IInstantiator _instantiator;
        private List<PlanetPresentation> _planetsPresentations;

        [Inject]
        private void Construct(PlanetsPresentation presentation, IInstantiator instantiator)
        {
            _instantiator = instantiator;
            _presentation = presentation;
            _planetsPresentations = new List<PlanetPresentation>(_planetsViews.Length);
        }

        private void Awake()
        {
            InitializeViews();
        }

        private void OnDestroy()
        {
            foreach (var planetPresentation in _planetsPresentations)
                planetPresentation.Dispose();
        }

        private void InitializeViews()
        {
            for (int i = 0; i < Mathf.Min(_planetsViews.Length, _presentation.Planets.Count); i++)
            {
                var view = _planetsViews[i];
                var planet = _presentation.Planets[i];
                
                var planetPresentation = _instantiator.Instantiate<PlanetPresentation>(new []{ planet });
                planetPresentation.Initialize();
                
                _planetsPresentations.Add(planetPresentation);
                
                view.Initialize(planetPresentation);
            }
        }
    }
}
