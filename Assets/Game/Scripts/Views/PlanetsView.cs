using System.Collections.Generic;
using Game.Presentation;
using Modules.Planets;
using R3;
using UnityEngine;
using Zenject;

namespace Game.Views
{
    public class PlanetsView : MonoBehaviour
    {
        [SerializeField] private PlanetView[] _planetsViews;

        private PlanetsPresentation _presentation;
        private Dictionary<IPlanet, PlanetView> _planetViewMap;

        [Inject]
        private void Construct(PlanetsPresentation presentation, IInstantiator instantiator)
        {
            _presentation = presentation;
            _planetViewMap = new Dictionary<IPlanet, PlanetView>(_planetsViews.Length);
        }

        private void Awake()
        {
            _presentation.OnPresentersCreated.Subscribe(_ => InitializeViews()).AddTo(this);
        }

        public PlanetView GetView(IPlanet planet) => _planetViewMap[planet];

        private void InitializeViews()
        {
            for (int i = 0; i < Mathf.Min(_planetsViews.Length, _presentation.Presentations.Count); i++)
            {
                var view = _planetsViews[i];
                var presentation = _presentation.Presentations[i];
                
                view.Initialize(presentation);
                
                _planetViewMap.Add(presentation.Planet, view);
            }
        }
    }
}
