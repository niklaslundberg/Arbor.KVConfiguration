using System;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;

using Arbor.KVConfiguration.Core;

namespace Arbor.KVConfiguration.Samples.ConsoleApp
{
    public class AppScenario3
    {
        public void Execute()
        {
            var collection = new NameValueCollection
                                 {
                                     { "urn:test:key", "a" },
                                     { "urn:another-key", "b" },
                                     { "urn:yet-another-key", "c" }
                                 };

            IKeyValueConfiguration appSettingsKeyValueConfiguration = new InMemoryKeyValueConfiguration(collection);

            KVConfigurationManager.Initialize(appSettingsKeyValueConfiguration);

            bool succeeded = true;

            Parallel.For(
                1,
                1001,
                index =>
                    {
                        Console.WriteLine("Loop index {0} on thread {1}", index, Thread.CurrentThread.ManagedThreadId);

                        string a = KVConfigurationManager.AppSettings["urn:test:key"];

                        if (a != "a")
                        {
                            Console.WriteLine("WRONG a in index {0}, value {1}", index, a);
                            succeeded = false;
                        }

                        string b = KVConfigurationManager.AppSettings["urn:another-key"];
                        if (b != "b")
                        {
                            Console.WriteLine("WRONG b in index {0}, value {1}", index, b);
                            succeeded = false;
                        }

                        string c = KVConfigurationManager.AppSettings["urn:yet-another-key"];
                        if (c != "c")
                        {
                            Console.WriteLine("WRONG c in index {0}, value {1}", index, c);
                            succeeded = false;
                        }
                    });

            Console.WriteLine(succeeded ? "OK" : "Failed");
        }
    }
}
