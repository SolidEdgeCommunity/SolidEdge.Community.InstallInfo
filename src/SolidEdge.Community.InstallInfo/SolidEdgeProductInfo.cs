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
                    new SolidEdgeProductInfo("Solid Edge V6", new Version(6, 0), new Version(10, 0), new Guid()),
                    new SolidEdgeProductInfo("Solid Edge V7", new Version(7, 0), new Version(11, 0), new Guid()),
                    new SolidEdgeProductInfo("Solid Edge V8", new Version(8, 0), new Version(11, 1), new Guid()),
                    new SolidEdgeProductInfo("Solid Edge V9", new Version(9, 0), new Version(12, 0), new Guid()),
                    new SolidEdgeProductInfo("Solid Edge V10", new Version(10, 0), new Version(13, 0), new Guid()),
                    new SolidEdgeProductInfo("Solid Edge V11", new Version(11, 0), new Version(13, 0), new Guid()),
                    new SolidEdgeProductInfo("Solid Edge V12", new Version(12, 0), new Version(14, 0), new Guid()),
                    new SolidEdgeProductInfo("Solid Edge V14", new Version(14, 0), new Version(14, 1), new Guid()),
                    new SolidEdgeProductInfo("Solid Edge V15", new Version(15, 0), new Version(15, 0), new Guid()),
                    new SolidEdgeProductInfo("Solid Edge V16", new Version(16, 0), new Version(16, 0), new Guid()),
                    new SolidEdgeProductInfo("Solid Edge V17", new Version(17, 0), new Version(16, 1), new Guid()),
                    new SolidEdgeProductInfo("Solid Edge V18", new Version(18, 0), new Version(17, 0), new Guid()),
                    new SolidEdgeProductInfo("Solid Edge V19", new Version(19, 0), new Version(18, 0), new Guid()),
                    new SolidEdgeProductInfo("Solid Edge V20", new Version(20, 0), new Version(18, 1), new Guid()),
                    new SolidEdgeProductInfo("Solid Edge ST", new Version(100, 0), new Version(19, 1), new Guid()),
                    new SolidEdgeProductInfo("Solid Edge ST2", new Version(102, 0), new Version(22, 0), new Guid("CC185D10-5C0E-40C3-91F2-63314BB365AF")),
                    new SolidEdgeProductInfo("Solid Edge ST3", new Version(103, 0), new Version(23, 0), new Guid("EA8B28A2-D84F-447E-B588-9C255F1EDC0A")),
                    new SolidEdgeProductInfo("Solid Edge ST4", new Version(104, 0), new Version(24, 0), new Guid("DE02B016-E096-437F-8D96-853BB36011D5")),
                    new SolidEdgeProductInfo("Solid Edge ST5", new Version(105, 0), new Version(25, 0), new Guid("6350353B-BE44-4E86-9B3F-CE2C77BDFAEC")),
                    new SolidEdgeProductInfo("Solid Edge ST6", new Version(106, 0), new Version(26, 0), new Guid("132B6ABB-431A-4DDA-8861-914AB7B0325A")),
                    new SolidEdgeProductInfo("Solid Edge ST7", new Version(107, 0), new Version(27, 0), new Guid("AB0F3228-D90C-4574-8A28-589483A68C93")),
                    new SolidEdgeProductInfo("Solid Edge ST8", new Version(108, 0), new Version(28, 0), new Guid("C69F7B10-60F2-476C-B0C1-4D61628462B7")),
                    new SolidEdgeProductInfo("Solid Edge ST9", new Version(109, 0), new Version(29, 0), new Guid("1E02E133-6790-460A-B9C7-9CEA71CB502A")),
                    new SolidEdgeProductInfo("Solid Edge ST10", new Version(110, 0), new Version(30, 0), new Guid("3D4C868F-5CCD-49F9-820C-DA31D714ABF6"))
                };
        }

        private SolidEdgeProductInfo(string name, Version version, Version parasolidVersion, Guid productCode)
        {
            Name = name;
            Version = version;
            ParasolidVersion = parasolidVersion;
            ProductCode = productCode;

            IsInstalled = NativeMethods.MsiQueryProductState(ProductCode.ToStringEnclosedWithBraces()) == InstallState.Default;
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
        /// 
        /// </summary>
        public Guid ProductCode { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsInstalled { get; private set; }

        /// <summary>
        /// Returns an array of known Solid Edge product information.
        /// </summary>
        public static SolidEdgeProductInfo[] AllKnown { get; private set; }
    }
}
