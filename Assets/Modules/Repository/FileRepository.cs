using System;
using System.IO;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.Encryption;
using Newtonsoft.Json.Linq;

namespace Game.Repository
{
    public sealed class FileRepository : IRepository
    {
        private readonly string _saveFolderPath;
        private readonly IEncryptor _encryptor;

        public FileRepository(string saveFolderPath, IEncryptor encryptor = null)
        {
            _saveFolderPath = saveFolderPath;
            _encryptor = encryptor;
        }

        public async UniTask<(bool, JObject)> Load(string version, CancellationToken ct = default)
        {
            if (!File.Exists(_saveFolderPath))
                return (false, null);

            try
            {
                var filePath = Path.Combine(_saveFolderPath, version);
                var bytes = await File.ReadAllBytesAsync(filePath, ct);
                
                if (_encryptor != null)
                    bytes = _encryptor.Decrypt(bytes);

                string json = Encoding.UTF8.GetString(bytes);
                return (true, JObject.Parse(json));
            }
            catch (Exception)
            {
                return (false, null);
            }
        }

        public async UniTask<bool> Save(string version, JObject data, CancellationToken ct = default)
        {
            string json = data.ToString();
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            if (_encryptor != null)
                bytes = _encryptor.Encrypt(bytes);

            try
            {
                var filePath = Path.Combine(_saveFolderPath, version);
                
                await File.WriteAllBytesAsync(filePath, bytes, ct);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
