namespace Collector
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Collector.Configuration;
    using Collector.Services;
    using Unity;

    /// <summary>
    /// Main entry point
    /// </summary>
    public class Program
    {
        private static IUnityContainer container = UnityConfiguration.Container;

        private static async Task Main(string[] args)
        {
            // should subscribe to some cancel event like keystroke
            var cancellationTokenSource = new CancellationTokenSource();

            // parse command line arguments
            var app = new Microsoft.Extensions.CommandLineUtils.CommandLineApplication();
            var workerOption = app.Option("--worker", "Run the process as worker", Microsoft.Extensions.CommandLineUtils.CommandOptionType.NoValue); app.HelpOption("--help");
            app.ShowHelp();
            app.Execute(args);

            if (workerOption.HasValue())
            {
                // if command line argument --worker has been set, we should only process the request until there are somethong to process
                Console.WriteLine("Executed as worker");
                var workerService = container.Resolve<IWorkerService>("Worker");
                await workerService.ProcessAsync(cancellationTokenSource.Token);
                // terminate the process when we are finished
                return;
            }

            Console.WriteLine("Executed as host");
            var host = container.Resolve<IWorkerService>("Host");
            await host.ProcessAsync(cancellationTokenSource.Token);
        }
    }
}