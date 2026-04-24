using System;
using Game.Repository;
using Modules.Encryption;
using UnityEngine;
using Zenject;

namespace Game.App
{
    [Serializable]
    public class RepositoryInstaller : Installer
    {
        [SerializeField] private string _uri = "http://127.0.0.1:8888";
        [SerializeField] private string _encryptionKey;
        
        public override void InstallBindings()
        {
            Container.Bind<IEncryptor>()
                     .To<AesEncryptor>()
                     .AsSingle()
                     .WithArguments(_encryptionKey);
            
            Container
                .BindInterfacesTo<RemoteRepository>()
                .AsSingle()
                .WithArguments(_uri);
        }
    }
}
