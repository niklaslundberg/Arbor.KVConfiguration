using System;
using System.IO;
using System.Linq;
using System.Reflection;

using Arbor.Aesculus.Core;

namespace Arbor.KVConfiguration.Tests.Integration
{
    internal static class VcsTestPathHelper
    {
        public static string FindVcsRootPath()
        {
            try
            {
                Assembly ncrunchAssembly = AppDomain.CurrentDomain.Load("NCrunch.Framework");

                Type ncrunchType =
                    ncrunchAssembly.GetTypes()
                        .FirstOrDefault(
                            type => type.Name.Equals("NCrunchEnvironment", StringComparison.InvariantCultureIgnoreCase));

                MethodInfo method = ncrunchType?.GetMethod("GetOriginalSolutionPath");

                string originalSolutionPath = method?.Invoke(null, null) as string;
                if (!string.IsNullOrWhiteSpace(originalSolutionPath))
                {
                    DirectoryInfo parent = new DirectoryInfo(originalSolutionPath).Parent;
                    // ReSharper disable once PossibleNullReferenceException
                    return VcsPathHelper.FindVcsRootPath(parent.FullName);
                }
            }
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch (Exception)
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
            {
            }

            return VcsPathHelper.FindVcsRootPath();
        }
    }
}
