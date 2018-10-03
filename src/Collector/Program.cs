namespace Collector
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Collector.Configuration;
    using Unity;

    /// <summary>
    /// Main entry point
    /// </summary>
    public class Program
    {
        private static IUnityContainer container = UnityConfiguration.Container;
        private static int processCount = 0;

        private static async Task Main(string[] args)
        {
            var cancellationTokenSource = new CancellationTokenSource();

            var app = new Microsoft.Extensions.CommandLineUtils.CommandLineApplication();
            var workerOption = app.Option("--worker", "Run the process as worker", Microsoft.Extensions.CommandLineUtils.CommandOptionType.NoValue); app.HelpOption("--help");
            app.ShowHelp();
            app.Execute(args);

            if (workerOption.HasValue())
            {
                Console.WriteLine("Executed as worker");
                var workerService = container.Resolve<IWorkerService>();
                await workerService.ProcessAsync(cancellationTokenSource.Token);
                Environment.Exit(0);
            }
            else
            {
                var client = container.Resolve<IAutoScaleProducerClient>();

                do
                {
                    var waitTime = await client.GetWaitTimeAsync(cancellationTokenSource.Token);

                    if (waitTime != null)
                    {
                        if (processCount < ConfigurationReader.Instance.Settings.MaxDegreeOfParallelism)
                        {
                            var workerProcess = Process.Start("dotnet", $"exec {Assembly.GetExecutingAssembly().Location} --worker");
                            Interlocked.Increment(ref processCount);
                            workerProcess.Exited += (s, o) => Interlocked.Decrement(ref processCount);
                        }

                        Console.WriteLine($"Host working for {waitTime}");
                        await Task.Delay(waitTime.Value, cancellationTokenSource.Token);
                    }
                    else
                    {
                        Console.WriteLine($"Host waiting for {ConfigurationReader.Instance.Settings.PollingTimeout}");
                        await Task.Delay(ConfigurationReader.Instance.Settings.PollingTimeout, cancellationTokenSource.Token);
                    }
                } while (!cancellationTokenSource.IsCancellationRequested);
            }
        }
    }
}