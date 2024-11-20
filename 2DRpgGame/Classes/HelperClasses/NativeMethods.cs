using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _2DRpgGame.Classes.HelperClasses
{
    internal static class NativeMethods
    {
        [DllImport("user32.dll")]
        internal static extern UnmanagedType SetProcessDPIAware();
    }
}
