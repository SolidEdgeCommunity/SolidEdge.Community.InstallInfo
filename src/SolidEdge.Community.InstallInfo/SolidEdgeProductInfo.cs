using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace SolidEdgeCommunity.InstallInfo
{
    /// <summary>
    /// Solid Edge version information.
    /// </summary>
    public class SolidEdgeProductInfo
    {
        static SolidEdgeProductInfo()
        {
            Refresh();
        }

        private SolidEdgeProductInfo()
        {
        }

        public static void Refresh()
        {
            var allKnown = new List<SolidEdgeProductInfo>();
            var streamName = $"{typeof(SolidEdgeProductInfo).FullName}.xml";
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(streamName))
            {
                var xDoc = XDocument.Load(stream);
                var productInfoElements = xDoc.Descendants("ProductInfo");

                foreach (var productInfoElement in productInfoElements)
                {
                    var platform = productInfoElement.Element(nameof(SolidEdgeProductInfo.Platform))?.Value;
                    var parasolidVersionString = productInfoElement.Element(nameof(SolidEdgeProductInfo.ParasolidVersion))?.Value;
                    var productCodeString = productInfoElement.Element(nameof(SolidEdgeProductInfo.ProductCode))?.Value;
                    var productName = productInfoElement.Element(nameof(SolidEdgeProductInfo.ProductName))?.Value;
                    var versionString = productInfoElement.Element(nameof(SolidEdgeProductInfo.Version))?.Value;

                    var productInfo = new SolidEdgeProductInfo()
                    {
                        Platform = platform,
                        ParasolidVersion = new Version(),
                        ProductCode = new Guid(),
                        ProductName = productName,
                        Version = new Version()
                    };

                    if (Guid.TryParse(productCodeString, out Guid productCode))
                    {
                        productInfo.ProductCode = productCode;
                    }

                    if (Version.TryParse(parasolidVersionString, out Version parasolidVersion))
                    {
                        productInfo.ParasolidVersion = parasolidVersion;
                    }

                    if (Version.TryParse(versionString, out Version version))
                    {
                        productInfo.Version = version;
                    }

                    allKnown.Add(productInfo);
                }
            }

            AllKnown = allKnown.ToArray();
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
            return $"{ProductName} ({Platform})";
        }

        /// <summary>
        /// Returns the product name of Solid Edge.
        /// </summary>
        public string ProductName { get; private set; }

        /// <summary>
        /// Returns the version of Solid Edge.
        /// </summary>
        public Version Version { get; private set; }

        /// <summary>
        /// Returns the parasolid version of Solid Edge.
        /// </summary>
        public Version ParasolidVersion { get; private set; }

        /// <summary>
        /// Unique identifier of product windows installer.
        /// </summary>
        public Guid ProductCode { get; private set; }

        /// <summary>
        /// Indicates a 32-bit or 64-bit installer.
        /// </summary>
        public string Platform { get; private set; }

        /// <summary>
        /// Returns the installation information associated with this product.
        /// </summary>
        public SolidEdgeInstallInfo InstallInfo
        {
            get
            {
                return SolidEdgeInstallInfo.All.FirstOrDefault(x => x.Version.Major == Version.Major);
            }
        }

        /// <summary>
        /// Determines if this product is the default installation.
        /// </summary>
        public bool IsDefault
        {
            get
            {
                var installInfo = InstallInfo;
                return installInfo == null ? false : installInfo.IsDefault;
            }
        }

        /// <summary>
        /// Determines if the product is installed on the local machine.
        /// </summary>
        public bool IsInstalled
        {
            get
            {
                return NativeMethods.MsiQueryProductState(ProductCode.ToStringEnclosedWithBraces()) == InstallState.Default;
            }
        }

        /// <summary>
        /// Returns an array of known Solid Edge product information.
        /// </summary>
        public static SolidEdgeProductInfo[] AllKnown { get; private set; }

        /// <summary>
        /// Returns an array of installed Solid Edge product information.
        /// </summary>
        public static SolidEdgeProductInfo[] AllInstalled
        {
            get
            {
                return AllKnown.Where(x => x.IsInstalled).ToArray();
            }
        }

        /// <summary>
        /// Returns the default installed Solid Edge product information.
        /// </summary>
        public static SolidEdgeProductInfo DefaultInstalled
        {
            get
            {
                return AllInstalled
                    .Where(x => x.InstallInfo != null)
                    .FirstOrDefault(x => x.InstallInfo.IsDefault);
            }
        }
    }
}
