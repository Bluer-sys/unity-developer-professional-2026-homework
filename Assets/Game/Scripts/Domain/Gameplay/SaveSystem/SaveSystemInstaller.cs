using System;
using System.Collections.Generic;
using Game.App;
using Modules.Entities;
using Modules.Extensions;
using SampleGame.Gameplay;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    [CreateAssetMenu(
        fileName = "SaveSystemInstaller",
        menuName = "Zenject/New Save System Installer"
    )]
    public class SaveSystemInstaller : ScriptableObjectInstaller
    {
        [SerializeField] 
        private RepositoryInstaller _repositoryInstaller;
        
        public override void InstallBindings()
        {
            Container.Install(_repositoryInstaller);
            
            Container.BindInterfacesTo<SaveManager>().AsSingle();
            
            Container.Bind<ISaveSerializer[]>()
                     .FromMethod(BindSerializers)
                     .AsSingle();
            
            Container.Bind<IReadOnlyDictionary<Type, IComponentSerializer>>()
                     .FromMethod(BindComponentSerializers)
                     .AsSingle();
        }

        private ISaveSerializer[] BindSerializers(InjectContext context)
        {
            return new ISaveSerializer[]
            {
                Instantiate<EntitiesSerializer>(),
            };
            
            T Instantiate<T>() => context.Container.Instantiate<T>();
        }

        private Dictionary<Type, IComponentSerializer> BindComponentSerializers(InjectContext context)
        {
            return new Dictionary<Type, IComponentSerializer>
            {
                { typeof(EntityWorld), Instantiate<EntityWorldSerializer>() },
                { typeof(Entity), Instantiate<EntitySerializer>() },
                { typeof(Countdown), Instantiate<CountdownSerializer>() }
            };

            T Instantiate<T>() => context.Container.Instantiate<T>();
        }
    }
}
