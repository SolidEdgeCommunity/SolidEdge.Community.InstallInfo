using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolidEdgeCommunity.InstallInfo
{
    public class ComponentCategoryInfo
    {
        internal ComponentCategoryInfo()
        {
        }

        public override string ToString()
        {
            return $"{CATID.ToStringEnclosedWithBraces()} '{NativeSymbolicName}' '{Description}'";
        }

        public Guid CATID { get; internal set; }
        public string Description { get; internal set; }
        public string NativeInclude { get; internal set; }
        public string NativeSymbolicName { get; internal set; }
    }
}
