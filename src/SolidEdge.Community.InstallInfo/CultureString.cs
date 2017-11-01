using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SolidEdgeCommunity.InstallInfo
{
    public class CultureString
    {
        public CultureString(CultureInfo culture, string value)
        {
            if (culture == null) throw new ArgumentNullException("culture");

            Culture = culture;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Culture} | {Value}";
        }

        public CultureInfo Culture { get; private set; }
        public string Value { get; private set; }
    }
}
