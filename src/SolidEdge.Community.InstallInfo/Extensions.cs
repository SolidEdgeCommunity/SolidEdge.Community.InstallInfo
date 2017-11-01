using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolidEdgeCommunity.InstallInfo
{
    static class GuidExtensions
    {
        public static string ToStringEnclosedWithBraces(this Guid guid)
        {
            return guid.ToString("B");
        }
    }
}
