using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolidEdgeCommunity.InstallInfo
{
    /// <summary>
    /// Solid Edge version information.
    /// </summary>
    public class SolidEdgeProductInfo
    {
        static SolidEdgeProductInfo()
        {
            AllKnown = new SolidEdgeProductInfo[]
                {
                    new SolidEdgeProductInfo("Solid Edge V6",new Version(6, 0), new Version(10, 0)),
                    new SolidEdgeProductInfo("Solid Edge V7",new Version(7, 0), new Version(11, 0)),
                    new SolidEdgeProductInfo("Solid Edge V8",new Version(8, 0), new Version(11, 1)),
                    new SolidEdgeProductInfo("Solid Edge V9",new Version(9, 0), new Version(12, 0)),
                    new SolidEdgeProductInfo("Solid Edge V10",new Version(10, 0), new Version(13, 0)),
                    new SolidEdgeProductInfo("Solid Edge V11",new Version(11, 0), new Version(13, 0)),
                    new SolidEdgeProductInfo("Solid Edge V12",new Version(12, 0), new Version(14, 0)),
                    new SolidEdgeProductInfo("Solid Edge V14",new Version(14, 0), new Version(14, 1)),
                    new SolidEdgeProductInfo("Solid Edge V15",new Version(15, 0), new Version(15, 0)),
                    new SolidEdgeProductInfo("Solid Edge V16",new Version(16, 0), new Version(16, 0)),
                    new SolidEdgeProductInfo("Solid Edge V17",new Version(17, 0), new Version(16, 1)),
                    new SolidEdgeProductInfo("Solid Edge V18",new Version(18, 0), new Version(17, 0)),
                    new SolidEdgeProductInfo("Solid Edge V19",new Version(19, 0), new Version(18, 0)),
                    new SolidEdgeProductInfo("Solid Edge V20",new Version(20, 0), new Version(18, 1)),
                    new SolidEdgeProductInfo("Solid Edge ST",new Version(100, 0), new Version(19, 1)),
                    new SolidEdgeProductInfo("Solid Edge ST2",new Version(102, 0), new Version(22, 0)),
                    new SolidEdgeProductInfo("Solid Edge ST3",new Version(103, 0), new Version(23, 0)),
                    new SolidEdgeProductInfo("Solid Edge ST4",new Version(104, 0), new Version(24, 0)),
                    new SolidEdgeProductInfo("Solid Edge ST5",new Version(105, 0), new Version(25, 0)),
                    new SolidEdgeProductInfo("Solid Edge ST6",new Version(106, 0), new Version(26, 0)),
                    new SolidEdgeProductInfo("Solid Edge ST7",new Version(107, 0), new Version(27, 0)),
                    new SolidEdgeProductInfo("Solid Edge ST8",new Version(108, 0), new Version(28, 0)),
                    new SolidEdgeProductInfo("Solid Edge ST9",new Version(109, 0), new Version(29, 0)),
                    new SolidEdgeProductInfo("Solid Edge ST10",new Version(110, 0), new Version(30, 0))
                };
        }

        private SolidEdgeProductInfo(string name, Version version, Version parasolidVersion)
        {
            Name = name;
            Version = version;
            ParasolidVersion = parasolidVersion;
        }

        /// <summary>
        /// Gets the parasolid version of a specific version of Solid Edge.
        /// </summary>
        /// <param name="solidEdgeVersion"></param>
        /// <returns>Parasolid version</returns>
        /// <remarks>Parasolid version information is gleaned from Solid Edge readme.</remarks>
        public static Version GetParasolidVersion(Version solidEdgeVersion)
        {
            var knownVersion = AllKnown.Where(x => x.Version.Major == solidEdgeVersion.Major).FirstOrDefault();
            return knownVersion == null ? new Version() : knownVersion.ParasolidVersion;
        }

        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Returns the product name of Solid Edge.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Returns the version of Solid Edge.
        /// </summary>
        public Version Version { get; private set; }

        /// <summary>
        /// Returns the parasolid version of Solid Edge.
        /// </summary>
        public Version ParasolidVersion { get; private set; }

        /// <summary>
        /// Returns an array of known Solid Edge product information.
        /// </summary>
        public static SolidEdgeProductInfo[] AllKnown { get; private set; }
    }
}
