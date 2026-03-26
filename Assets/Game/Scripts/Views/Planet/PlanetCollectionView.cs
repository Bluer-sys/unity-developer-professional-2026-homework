using Game.Presentation;
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
            
            ConstructViews();
        }

        private void ConstructViews()
        {
            var presentations = _presentation.PlanetPresentations;

            for (int i = 0; i < Mathf.Min(_planetsViews.Length, presentations.Count); i++)
            {
                var view = _planetsViews[i];
                var presentation = presentations[i];
                
                view.Construct(presentation);
            }
        }
    }
}
