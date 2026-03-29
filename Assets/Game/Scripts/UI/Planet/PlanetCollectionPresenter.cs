using System.Collections.Generic;
using Modules.Planets;
using UnityEngine;
using Zenject;

namespace Game.UI
{
    public class PlanetCollectionPresenter : MonoBehaviour
    {
        [Inject]
        private void Construct(
                IReadOnlyList<IPlanet> planets, 
                IReadOnlyList<PlanetPresenter> planetPresenters,
                MoneyPresenter moneyPresenter,
                PlanetPopupPresenter planetPopupPresenter
            )
        {
            for (int i = 0; i < Mathf.Min(planets.Count, planetPresenters.Count); i++)
            {
                var model = planets[i];
                var presenter = planetPresenters[i];

                presenter.Construct(model, moneyPresenter, planetPopupPresenter);
            }
        }
    }
}
