namespace Collector.Configuration
{
    using System;
    using Collector.Client;
    using Collector.Services;
    using Unity;
    using Unity.Lifetime;

    /// <summary>
    /// Static unity configuration
    /// </summary>
    public static class UnityConfiguration
    {
        private static readonly Lazy<IUnityContainer> containerFactory = new Lazy<IUnityContainer>(GetConfiguredContainer);

        /// <summary>
        /// Gets the configured container
        /// </summary>
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
                .RegisterType<IJobProcessor, MockJobProcessor>(new HierarchicalLifetimeManager())
                .RegisterType<IWorkerService, HostService>("Host", new ContainerControlledLifetimeManager())
                .RegisterType<IWorkerService, WorkerService>("Worker", new TransientLifetimeManager());
        }
    }
}