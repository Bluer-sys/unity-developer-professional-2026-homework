using System;
using Game.Repository;
using UnityEngine;
using Zenject;

namespace Game.App
{
    [Serializable]
    public class RepositoryInstaller : Installer
    {
        [SerializeField] private string _uri = "http://127.0.0.1:8888";
        
        public override void InstallBindings()
        {
            Container
                .BindInterfacesTo<RemoteRepository>()
                .AsSingle()
                .WithArguments(_uri);
        }
    }
}
