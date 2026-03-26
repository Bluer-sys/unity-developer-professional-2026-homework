using System.Collections.Generic;
using Game.Presentation;
using R3;
using UnityEngine;
using Zenject;

namespace Game.Views
{
    public class PlanetCollectionView : MonoBehaviour
    {
        [SerializeField] private PlanetView[] _planetsViews;

        private PlanetCollectionPresentation _presentation;

        [Inject]
        private void Construct(PlanetCollectionPresentation presentation, IInstantiator instantiator)
        {
            _presentation = presentation;
        }

        private void Awake()
        {
            _presentation.OnPresentersCreated.Subscribe(InitializeViews).AddTo(this);
        }

        private void InitializeViews(IReadOnlyList<PlanetPresentation> presentations)
        {
            for (int i = 0; i < Mathf.Min(_planetsViews.Length, presentations.Count); i++)
            {
                var view = _planetsViews[i];
                var presentation = presentations[i];
                
                view.Initialize(presentation);
            }
        }
    }
}
