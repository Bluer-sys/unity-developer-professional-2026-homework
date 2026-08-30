using Fusion;
using Game.Core;
using UnityEngine;

namespace Game
{
    public class GameOverPresenter : MonoBehaviour
    {
        [SerializeField] private HealthComponent _portalHealth;
        [SerializeField] private GameObject _losePopup;

        private void OnEnable()
        {
            _portalHealth.OnDeath += SetLose;
        }

        private void OnDisable()
        {
            _portalHealth.OnDeath -= SetLose;
        }

        private void SetLose(NetworkObject obj)
        {
            _losePopup.SetActive(true);
        }
    }
}
