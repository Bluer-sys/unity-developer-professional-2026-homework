using Fusion;
using Game.Camera;
using Game.Money;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts
{
    public class FusionBootstrap : MonoBehaviour
    {
        [SerializeField] private NetworkRunner _runner;
        [SerializeField] private CameraFollower _cameraFollower;

        private async void Start()
        {
            var sceneInfo = new NetworkSceneInfo();
            sceneInfo.AddSceneRef(SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex));

            var result = await _runner.StartGame(new StartGameArgs
            {
                GameMode = GameMode.AutoHostOrClient,
                SessionName = "SampleSession",
                PlayerCount = 2,
                Scene = sceneInfo,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });

            if (!result.Ok)
            {
                Debug.LogError($"Fusion StartGame failed: {result.ShutdownReason} - {result.ErrorMessage}");
                return;
            }
            
            _runner.AddGlobal(_cameraFollower);
        }
    }
}
