using Game.GameObjects.Core;
using UnityEngine;

namespace Game.GameContext.Player
{
    public class PlayerDeathObserver : MonoBehaviour
    {
        [SerializeField] private HealthComponent _health;
        [SerializeField] private PlayerFireController _playerFireController;
        [SerializeField] private PlayerMovementController _playerMovementController;
        [SerializeField] private GameObject _player;

        private void OnEnable()
        {
            _health.OnDead += OnDead;
        }

        private void OnDisable()
        {
            _health.OnDead -= OnDead;
        }

        private void OnDead()
        {
            _player.SetActive(false);
            _playerFireController.enabled = false;
            _playerMovementController.enabled = false;
        }
    }
}
