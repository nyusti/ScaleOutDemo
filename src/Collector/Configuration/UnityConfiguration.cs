namespace Collector.Configuration
{
    using System;
    using Unity;
    using Unity.Lifetime;

    public static class UnityConfiguration
    {
        private static readonly Lazy<IUnityContainer> containerFactory = new Lazy<IUnityContainer>(GetConfiguredContainer);

        public static IUnityContainer Container => containerFactory.Value;

        private static IUnityContainer GetConfiguredContainer()
        {
            var container = new UnityContainer();
            DesignTimeConfiguration(container);
            return container;
        }

        private static void DesignTimeConfiguration(IUnityContainer container)
        {
            container
                .RegisterType<IAutoScaleProducerClient, AutoScaleProducerClient>(new ContainerControlledLifetimeManager())
                .RegisterType<IWorkerService, WorkerService>(new TransientLifetimeManager());
        }
    }
}