using Game.Presentation;
using Modules.Planets;
using Modules.UI;
using R3;
using UnityEngine;
using Zenject;

namespace Game.Views
{
    public class PlanetGatherIncomeView : MonoBehaviour
    {
        [SerializeField] private ParticleAnimator _particleAnimator;

        private PlanetGatherIncomePresentation _presentation;
        private PlanetsView _planetsView;
        private MoneyPanelView _moneyPanelView;

        [Inject]
        private void Construct(PlanetGatherIncomePresentation presentation, 
                               MoneyPanelView moneyPanelView, 
                               PlanetsView planetsView)
        {
            _presentation = presentation;
            _moneyPanelView = moneyPanelView;
            _planetsView = planetsView;
        }

        private void Awake()
        {
            _presentation.OnGatherRequested.Subscribe(PlayGatherAnimation).AddTo(this);
        }

        private void PlayGatherAnimation(IPlanet planet)
        {
            var planetView = _planetsView.GetView(planet);
            
            _particleAnimator.Emit(planetView.CoinPosition, _moneyPanelView.CoinPosition, 
                onFinished: () => _presentation.EndGather(planet));
        }
    }
}
