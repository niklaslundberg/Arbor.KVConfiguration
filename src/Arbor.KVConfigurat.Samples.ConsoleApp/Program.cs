using System;

namespace Arbor.KVConfigurat.Samples.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            new AppScenario1().Execute();

            Console.WriteLine(new string('*', 50));

            new AppScenario2().Execute();

            Console.WriteLine(new string('*', 50));

            new AppScenario3().Execute();
        }
    }
}
