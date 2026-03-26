using System.Collections.Generic;
using Game.Presentation;
using R3;
using UnityEngine;
using Zenject;

namespace Game.Views
{
    public class PlanetCollectionView : MonoBehaviour
    {
        private PlanetCollectionPresentation _presentation;
        private PlanetView[] _planetsViews;

        [Inject]
        private void Construct(
            PlanetCollectionPresentation presentation, 
            PlanetView[] planetViews, 
            IInstantiator instantiator)
        {
            _presentation = presentation;
            _planetsViews = planetViews;
            _presentation.OnPresentersCreated.Subscribe(ConstructViews).AddTo(this);
        }

        private void ConstructViews(IReadOnlyList<PlanetPresentation> presentations)
        {
            for (int i = 0; i < Mathf.Min(_planetsViews.Length, presentations.Count); i++)
            {
                var view = _planetsViews[i];
                var presentation = presentations[i];
                
                view.Construct(presentation);
            }
        }
    }
}
