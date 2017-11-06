using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SolidEdgeCommunity.InstallInfo
{
    internal static class NativeMethods
    {
        [DllImport("msi.dll", CharSet = CharSet.Unicode, ExactSpelling = false)]
        public static extern InstallState MsiQueryProductState(string szProduct);
    }
}
