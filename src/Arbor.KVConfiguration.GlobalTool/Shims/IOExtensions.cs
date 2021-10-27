using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if NET5_0_OR_GREATER
#else
namespace System.IO.Extensions
{
    internal static class File
    {
        public static bool Exists(string path)
        {
            return System.IO.File.Exists(path);
        }

        public static Task WriteAllTextAsync(string path, string contents, Encoding? encoding = null)
        {
            System.IO.File.WriteAllText(path, contents, encoding ?? Encoding.UTF8);

            return Task.CompletedTask;
        }

        public static Task<string> ReadAllTextAsync(string path, Encoding? encoding = null)
        {
            return Task.FromResult(System.IO.File.ReadAllText(path, encoding ?? Encoding.UTF8));
        }
    }
}
#endif
