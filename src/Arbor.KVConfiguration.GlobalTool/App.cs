using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arbor.KVConfiguration.Schema.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Arbor.KVConfiguration.GlobalTool
{
    public sealed class App : IAsyncDisposable
    {
        private App(
            IHost host,
            ILogger logger,
            string[] args,
            IReadOnlyDictionary<string, string> variables)
        {
            Host = host;
            Logger = logger;
            Args = args;
            Variables = variables;
        }

        public IHost Host { get; }

        public ILogger Logger { get; }

        public string[] Args { get; }

        public IReadOnlyDictionary<string, string> Variables { get; }

        public async ValueTask DisposeAsync()
        {
            Logger.Debug("Disposing host");
            Host.Dispose();
        }

        public static async Task<int> CreateAndRunAsync(string[] args, IReadOnlyDictionary<string, string> variables)
        {
            var loggerConfiguration = new LoggerConfiguration()
                .WriteTo.Console();

            if (args.Any(arg => arg.Equals(AppConstants.DebugArg)))
            {
                loggerConfiguration = loggerConfiguration
                    .MinimumLevel.Debug();
            }

            using var logger = loggerConfiguration.CreateLogger();

            App app;

            try
            {
                logger.Debug("Building app");
                app = await BuildApp(args, variables, logger);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Could not create host");
                return 1;
            }

            try
            {
                logger.Debug("Starting app");

                int exitCode = await app.RunAsync();
                logger.Debug("Exiting with exit coed {ExitCode}", exitCode);
                return exitCode;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Application run failed");
                throw;
            }
            finally
            {
                await app.DisposeAsync();
            }
        }

        private async Task<int> RunAsync()
        {
            Logger.Information("Running application");

            string[] usedArgs = Args;

            if (Environment.UserInteractive && Debugger.IsAttached && usedArgs.Length == 0)
            {
                var args = new List<string>();

                while (true)
                {
                    string readLine = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(readLine))
                    {
                        break;
                    }

                    args.Add(readLine);
                }

                usedArgs = args.ToArray();
            }

            if (usedArgs.Length == 0)
            {
                Logger.Error("Missing required args");
                return 3;
            }

            var newPairs = Host.Services.GetRequiredService<ArgParser>().Parse(usedArgs.Skip(1));

            if (newPairs.IsDefaultOrEmpty)
            {
                Logger.Error("No value pairs defined");
                return 2;
            }

            string file = usedArgs.First();

            var kvPairs = newPairs.ToList();

            Logger.Debug("Adding {ExistingCount} new values", kvPairs.Count);

            if (File.Exists(file))
            {
                Logger.Debug("Found existing file '{File}'", file);
                string content = await File.ReadAllTextAsync(file, Encoding.UTF8);
                var items = JsonConfigurationSerializer.Deserialize(content);

                var oldValuesToAdd = items.Keys.Where(oldPair =>
                    !newPairs.Any(newPair => oldPair.Key.Equals(newPair.Key, StringComparison.OrdinalIgnoreCase)))
                    .ToArray();

                Logger.Debug("Adding {ExistingCount} existing values", oldValuesToAdd.Length);

                foreach (var oldValue in oldValuesToAdd)
                {
                    if (oldValue.Value is null)
                    {
                        continue;
                    }

                    kvPairs.Add(new KeyValuePair<string, string>(oldValue.Key, oldValue.Value));
                }
            }

            var sorted = kvPairs.OrderBy(pair => pair.Key);

            var configurationItems = new ConfigurationItems("1.0",
                sorted
                    .Select(pair => new KeyValue(pair.Key, pair.Value, null))
                    .ToImmutableArray());

            string json = JsonConfigurationSerializer.Serialize(configurationItems);

            try
            {
                await File.WriteAllTextAsync(file, json, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Could not write items to file '{File}'", file);
                throw;
            }

            Logger.Debug("Successfully written file '{File}'", file);

            return 0;
        }

        private static async Task<App> BuildApp(string[] args,
            IReadOnlyDictionary<string, string> variables,
            ILogger logger)
        {
            logger.Debug("Creating host");

            var hostBuilder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton(logger);
                    services.AddSingleton<ArgParser>();
                }).UseSerilog(logger);

            var host = hostBuilder.Build();

            return new App(host, logger, args, variables);
        }
    }
}