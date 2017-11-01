using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolidEdgeCommunity.InstallInfo
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    class NativeSymbolicNameAttributeAttribute : Attribute
    {
        public NativeSymbolicNameAttributeAttribute(string name, string include)
        {
            Name = name;
            Include = include;
        }

        public string Name { get; private set; }
        public string Include { get; private set; }
    }
}
