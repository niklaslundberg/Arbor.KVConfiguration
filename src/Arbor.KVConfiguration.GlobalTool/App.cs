using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
#if NET5_0_OR_GREATER
using System.IO;
#else
using System.IO.Extensions;
#endif
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arbor.KVConfiguration.Schema.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;

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

        public ValueTask DisposeAsync()
        {
            Logger.Debug("Disposing host");
            Host.Dispose();
            return default;
        }

        public static async Task<int> CreateAndRunAsync(string[] args, IReadOnlyDictionary<string, string> variables)
        {
            LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
                .WriteTo.Console();

            if (args.Any(arg => arg.Equals(AppConstants.DebugArg)))
            {
                loggerConfiguration = loggerConfiguration
                    .MinimumLevel.Debug();
            }

            using Logger logger = loggerConfiguration.CreateLogger();

            App app;

            try
            {
                logger.Debug("Building app");
                app = BuildApp(args, variables, logger);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Could not create host");
                return 1;
            }

            try
            {
                logger.Debug("Starting app");

                int exitCode = await app.RunAsync().ConfigureAwait(false);
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
                await app.DisposeAsync().ConfigureAwait(false);
            }
        }

        private async Task<int> RunAsync()
        {
            Logger.Debug("Running application");

            string[] usedArgs = GetArgs();

            if (usedArgs.Length == 0)
            {
                Logger.Error("Missing required args");
                ShowUsage();
                return 3;
            }

            ImmutableArray<KeyValuePair<string, string>> newPairs =
                Host.Services.GetRequiredService<ArgParser>().Parse(usedArgs.Skip(1));

            if (newPairs.IsDefaultOrEmpty)
            {
                Logger.Error("No value pairs defined");
                ShowUsage();
                return 2;
            }

            string file = usedArgs.First();

            var kvPairs = newPairs.ToList();

            Logger.Debug("Adding {ExistingCount} new values", kvPairs.Count);

            if (File.Exists(file))
            {
                Logger.Debug("Found existing file '{File}'", file);
                string content = await File.ReadAllTextAsync(file, Encoding.UTF8).ConfigureAwait(false);
                ConfigurationItems items = JsonConfigurationSerializer.Deserialize(content);

                KeyValue[] oldValuesToAdd = items.Keys.Where(oldPair =>
                        !newPairs.Any(newPair => oldPair.Key.Equals(newPair.Key, StringComparison.OrdinalIgnoreCase)))
                    .ToArray();

                Logger.Debug("Adding {ExistingCount} existing values", oldValuesToAdd.Length);

                foreach (KeyValue oldValue in oldValuesToAdd)
                {
                    if (oldValue.Value is null)
                    {
                        continue;
                    }

                    kvPairs.Add(new KeyValuePair<string, string>(oldValue.Key, oldValue.Value));
                }
            }

            IOrderedEnumerable<KeyValuePair<string, string>> sorted = kvPairs.OrderBy(pair => pair.Key);

            var configurationItems = new ConfigurationItems("1.0",
                sorted
                    .Select(pair => new KeyValue(pair.Key, pair.Value, null))
                    .ToImmutableArray());

            string json = JsonConfigurationSerializer.Serialize(configurationItems);

            try
            {
                await File.WriteAllTextAsync(file, json, Encoding.UTF8).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Could not write items to file '{File}'", file);
                throw;
            }

            Logger.Debug("Successfully written file '{File}'", file);

            return 0;
        }

        private void ShowUsage() => Logger.Information(
            "usage: {{fullPathFileToWrite}} {{argKey}}={{argValue}} Example: {ExampleFileName} {ExampleKey1}={ExampleValue1} {ExampleKey2}={ExampleValue2}",
            "c:\\applicationMetadata.json", "myKey", "myValue", "myKey2", "myValue2");

        private string[] GetArgs()
        {
            string[] usedArgs = Args;

            if (Environment.UserInteractive && Debugger.IsAttached && usedArgs.Length == 0)
            {
                Logger.Information("Enter args");
                var args = new List<string>();

                while (true)
                {
                    string? readLine = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(readLine))
                    {
                        break;
                    }

                    args.Add(readLine);
                }

                usedArgs = args.ToArray();
            }

            return usedArgs;
        }

        private static App BuildApp(string[] args,
            IReadOnlyDictionary<string, string> variables,
            ILogger logger)
        {
            logger.Debug("Creating host");

            IHostBuilder hostBuilder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton(logger);
                    services.AddSingleton<ArgParser>();
                }).UseSerilog(logger);

            IHost host = hostBuilder.Build();

            return new App(host, logger, args, variables);
        }
    }
}