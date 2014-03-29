using System;
using ITE.ConvertModule.Services;
using ITE.ConvertModule.Views;
using ITE.Infrastructure;
using ITE.Infrastructure.Interfaces;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace ITE.ConvertModule
{
    public class Module : IModule
    {
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer container;

        public Module(IUnityContainer container, IRegionManager regionManager)
        {
            if (regionManager == null)
                throw new ArgumentNullException("regionManager");

            if (container == null)
                throw new ArgumentNullException("container");

            this.regionManager = regionManager;
            this.container = container;
        }

        public void Initialize()
        {
            container.RegisterType<IConvertService, ConvertService>(new ContainerControlledLifetimeManager());

            regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(ConvertView))
                .RegisterViewWithRegion(RegionNames.MainToolBarRegion, typeof(ConvertCommandsView));
        }
    }
}
