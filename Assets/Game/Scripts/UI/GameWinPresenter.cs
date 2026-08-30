using UnityEngine;

namespace Game
{
    public class GameWinPresenter : MonoBehaviour
    {
        [SerializeField] private GameResult _gameResult;
        [SerializeField] private GameObject _winPopup;

        private void OnEnable()
        {
            _gameResult.OnWin += SetLose;
        }

        private void OnDisable()
        {
            _gameResult.OnWin -= SetLose;
        }

        private void SetLose()
        {
            _winPopup.SetActive(true);
        }
    }
}
