using System;

namespace Arbor.KVConfigurat.Samples.ConsoleApp
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            new AppScenario1().Execute();

            Console.WriteLine(new string('*', 50));

            new AppScenario2().Execute();

            Console.WriteLine(new string('*', 50));

            new AppScenario3().Execute();

            new AppScenario4().Execute();

            return 0;
        }
    }
}
