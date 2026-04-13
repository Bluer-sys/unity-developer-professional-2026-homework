using System;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Game.Repository
{
    public class RemoteRepository : IRepository
    {
        private readonly string _uri;

        public RemoteRepository(string uri)
        {
            _uri = uri;
        }
        
        public async UniTask<bool> Save(string version, JObject data, CancellationToken cancellationToken = default)
        {
            var body = new JObject()
            {
                ["data"] = data.ToString()
            };
            
            byte[] bytes = Encoding.UTF8.GetBytes(body.ToString());

            var request = new UnityWebRequest($"{_uri}/save?version={version}","PUT")
            {
                uploadHandler = new UploadHandlerRaw(bytes),
                downloadHandler = new DownloadHandlerBuffer(),
            };
            
            request.SetRequestHeader("Content-Type", "application/json");

            try
            {
                await request.SendWebRequest().WithCancellation(cancellationToken);
            }
            catch (Exception e)
            {
                Debug.LogError($"Save failed: {e}");
                return false;
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Save failed: {request.error}");
                return false;
            }

            Debug.Log($"Save completed: {data}");
            return true;
        }

        public async UniTask<(bool, JObject)> Load(string version, CancellationToken cancellationToken = default)
        {
            var request = new UnityWebRequest($"{_uri}/load?version={version}", "GET")
            {
                downloadHandler = new DownloadHandlerBuffer()
            };

            try
            {
                await request.SendWebRequest().WithCancellation(cancellationToken);
            }
            catch (Exception e)
            {
                Debug.LogError($"Load failed: {e}");
                return (false, null);
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Load failed: {request.error}");
                return (false, null);
            }
            
            var responce = JObject.Parse(request.downloadHandler.text);
            var jsonText = responce["data"]?.ToString();
            
            if(string.IsNullOrEmpty(jsonText))
                return (false, null);
            
            var data = JObject.Parse(jsonText);
            
            return (true, data);
        }
    }
}
