using System;
using System.Collections.Generic;
using Game.App;
using Modules.Entities;
using Modules.Extensions;
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
                context.Container.Instantiate<EntitiesSerializer>()
            };
        }

        private Dictionary<Type, IComponentSerializer> BindComponentSerializers(InjectContext context)
        {
            return new Dictionary<Type, IComponentSerializer>
            {
                { typeof(Entity), context.Container.Instantiate<EntitySerializer>(new object[]
                    {
                        new Dictionary<Type, IComponentSerializer>
                        {
                            { typeof(Countdown), Instantiate<CountdownSerializer>() },
                            { typeof(Health), Instantiate<HealthSerializer>() },
                            { typeof(Team), Instantiate<TeamSerializer>() },
                            { typeof(ResourceBag), Instantiate<ResourceBagSerializer>() },
                            { typeof(DestinationPoint), Instantiate<DestinationPointSerializer>() },
                            { typeof(TargetObject), Instantiate<TargetObjectSerializer>() },
                            { typeof(ProductionOrder), Instantiate<ProductionOrderSerializer>() },
                        }
                    })
                },
            };

            T Instantiate<T>() => context.Container.Instantiate<T>();
        }
    }
}
