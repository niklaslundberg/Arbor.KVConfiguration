using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if NET5_0_OR_GREATER
#else
namespace System
{
    public sealed class NotNullWhen : System.Attribute
    {
        public NotNullWhen(bool _)
        {
        }
    }
}

#endif