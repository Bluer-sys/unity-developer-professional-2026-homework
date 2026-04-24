using System;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.Encryption;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Game.Repository
{
    public class RemoteRepository : IRepository
    {
        private readonly string _uri;
        private readonly IEncryptor _encryptor;

        public RemoteRepository(string uri, IEncryptor encryptor)
        {
            _uri = uri;
            _encryptor = encryptor;
        }
        
        public async UniTask<bool> Save(string version, JObject data, CancellationToken cancellationToken = default)
        {
            var body = new JObject()
            {
                ["data"] = data.ToString()
            };
            
            var bytes = Encoding.UTF8.GetBytes(_encryptor.Encrypt(body.ToString()));
            
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

            var responce = _encryptor.Decrypt(request.downloadHandler.text);
            
            var jObject = JObject.Parse(responce);
            var jsonText = jObject["data"]?.ToString();
            
            if(string.IsNullOrEmpty(jsonText))
                return (false, null);
            
            var data = JObject.Parse(jsonText);
            
            return (true, data);
        }
    }
}
