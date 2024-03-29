﻿using System.Threading.Tasks;
using Arbor.Primitives;

namespace Arbor.KVConfiguration.GlobalTool
{
    internal static class Program
    {
        private static Task<int> Main(string[] args) =>
            App.CreateAndRunAsync(args, EnvironmentVariables.GetEnvironmentVariables().Variables);
    }
}