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
        menuName = "Zenject/New Save System Installer")]
    public class SaveSystemInstaller : ScriptableObjectInstaller
    {
        [SerializeField]
        private RepositoryInstaller _repositoryInstaller;

        public override void InstallBindings()
        {
            Container.Install(_repositoryInstaller);

            Container.BindInterfacesTo<SaveManager>()
                     .AsSingle();

            Container.Bind<ISaveSerializer[]>()
                     .FromMethod(BindSerializers)
                     .AsSingle();
        }

        private ISaveSerializer[] BindSerializers(InjectContext context)
        {
            var container = context.Container;

            return new ISaveSerializer[]
            {
                container.Instantiate<EntityWorldSerializer>(new Dictionary<Type, ISaveSerializer>
                {
                    { typeof(Countdown), container.Instantiate<CountdownSerializer>() },
                    { typeof(Health), container.Instantiate<HealthSerializer>() },
                    { typeof(Team), container.Instantiate<TeamSerializer>() },
                    { typeof(ResourceBag), container.Instantiate<ResourceBagSerializer>() },
                    { typeof(DestinationPoint), container.Instantiate<DestinationPointSerializer>() },
                    { typeof(TargetObject), container.Instantiate<TargetObjectSerializer>() },
                    { typeof(ProductionOrder), container.Instantiate<ProductionOrderSerializer>() },
                })
            };
        }
    }
}
