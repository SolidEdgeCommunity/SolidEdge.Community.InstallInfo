using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SolidEdgeCommunity.InstallInfo
{
    public class SolidEdgeInstallInfo
    {
        static SolidEdgeInstallInfo()
        {
            Refresh();
        }

        private SolidEdgeInstallInfo()
        {
            // Default to empty.
            ParasolidVersion = new Version();
            Version = new Version();
        }

        public static void Refresh()
        {
            Default = null;
            All = new SolidEdgeInstallInfo[] { };

            var all = new List<SolidEdgeInstallInfo>();

            if (Environment.Is64BitOperatingSystem)
            {
                all.AddRange(ProcessLocalMachine(RegistryView.Registry64));
                all.AddRange(ProcessLocalMachine(RegistryView.Registry32));
            }
            else
            {
                all.AddRange(ProcessLocalMachine(RegistryView.Registry32));
            }

            // Remove duplicates. For example, ST8 x64 writes to
            // HKEY_LOCAL_MACHINE\SOFTWARE\Unigraphics Solutions\Solid Edge\Version 108
            // and
            // HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Unigraphics Solutions\Solid Edge\Version 108.
            All = all.GroupBy(x => x.Version).Select(x => x.First()).ToArray();

            Default = All.FirstOrDefault(x => x.IsDefault);
        }

        static IEnumerable<SolidEdgeInstallInfo> ProcessLocalMachine(RegistryView registryView)
        {
            var list = new List<SolidEdgeInstallInfo>();

            using (var localMachineKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView))
            {
                using (var solidEdgeKey = localMachineKey.OpenSubKey(@"SOFTWARE\Unigraphics Solutions\Solid Edge", false))
                {
                    if (solidEdgeKey != null)
                    {
                        foreach (var versionName in solidEdgeKey.GetSubKeyNames())
                        {
                            using (var versionKey = solidEdgeKey.OpenSubKey(versionName, false))
                            {
                                using (var currentVersionKey = versionKey.OpenSubKey("CurrentVersion", false))
                                {
                                    if (currentVersionKey != null)
                                    {
                                        var installInfo = new SolidEdgeInstallInfo()
                                        {
                                            BaseProduct = $"{currentVersionKey.GetValue("BaseProduct")}",
                                            Company = $"{currentVersionKey.GetValue("Company")}",
                                            Description = $"{currentVersionKey.GetValue("Description")}",
                                            HelpPath = $"{currentVersionKey.GetValue("HelpPath")}",
                                            LanguageId = (int)currentVersionKey.GetValue("InstalledLanguage", 0),
                                            InstallMode = $"{currentVersionKey.GetValue("InstallMode")}",
                                            InstallPath = $"{currentVersionKey.GetValue("InstallPath")}",
                                            Owner = $"{currentVersionKey.GetValue("Owner")}",
                                            PreferencesPath = $"{currentVersionKey.GetValue("PreferencesPath")}",
                                            ProductCode = $"{currentVersionKey.GetValue("ProductCode")}",
                                            RegistryPath = $"{versionKey.Name}",
                                            VersionString = $"{currentVersionKey.GetValue("Build")}"
                                        };

                                        installInfo.Version = ParseVersionFromBuildString(installInfo.VersionString);
                                        installInfo.ParasolidVersion = SolidEdgeProductInfo.GetParasolidVersion(installInfo.Version);

                                        installInfo.LanguageCulture = CultureInfo.GetCultures(CultureTypes.AllCultures)
                                            .Where(x => x.LCID == installInfo.LanguageId)
                                            .FirstOrDefault();

                                        list.Add(installInfo);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (list.Count == 1)
            {
                list.First().IsDefault = true;
            }

            return list;
        }

        private static Version ParseVersionFromBuildString(string build)
        {
            var buildTokens = build.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (buildTokens.Length > 0)
            {
                if (System.Version.TryParse(buildTokens[0], out Version version))
                {
                    return version;
                }
            }

            return new Version();
        }

        /// <summary>
        /// Returns the version string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return VersionString;
        }

        /// <summary>
        /// Returns information about the default Solid Edge install.
        /// </summary>
        public static SolidEdgeInstallInfo Default { get; private set; }

        /// <summary>
        /// Returns information about the all Solid Edge installs.
        /// </summary>
        public static SolidEdgeInstallInfo[] All { get; private set; }

        /// <summary>
        /// Returns the value of the BaseProduct registry value.
        /// </summary>
        public string BaseProduct { get; private set; }

        /// <summary>
        /// Returns the value of the Company registry value.
        /// </summary>
        public string Company { get; private set; }

        /// <summary>
        /// Returns the value of the Description registry value.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Returns the value of the HelpPath registry value.
        /// </summary>
        public string HelpPath { get; private set; }

        /// <summary>
        /// Returns the value of the InstalledLanguage registry value.
        /// </summary>
        public int LanguageId { get; private set; }

        /// <summary>
        /// Returns culture information for the installed language.
        /// </summary>
        public CultureInfo LanguageCulture { get; private set; }

        /// <summary>
        /// Returns true if this is the default installed instance.
        /// </summary>
        public bool IsDefault { get; private set; }

        /// <summary>
        /// Returns the value of the InstallMode registry value.
        /// </summary>
        public string InstallMode { get; private set; }

        /// <summary>
        /// Returns the value of the InstallPath registry value.
        /// </summary>
        public string InstallPath { get; private set; }

        /// <summary>
        /// Returns the value of the Owner registry value.
        /// </summary>
        public string Owner { get; private set; }

        /// <summary>
        /// Returns the Parasolid version of Solid Edge.
        /// </summary>
        public Version ParasolidVersion { get; private set; }

        /// <summary>
        /// Returns the value of the PreferencesPath registry value.
        /// </summary>
        public string PreferencesPath { get; private set; }

        /// <summary>
        /// Returns the value of the ProduceCode registry value.
        /// </summary>
        public string ProductCode { get; private set; }
        
        /// <summary>
        /// Returns the full registry path of the component.
        /// </summary>
        public string RegistryPath { get; private set; }

        /// <summary>
        /// Returns a Version object representing the version of Solid Edge.
        /// </summary>
        /// <remarks>
        /// Version information parsed from Build registry value.
        /// </remarks>
        public Version Version { get; private set; }

        /// <summary>
        /// Returns the value of the Version registry value.
        /// </summary>
        public string VersionString { get; private set; }

        /// <summary>
        /// Returns the major version of Solid Edge.
        /// </summary>
        public int MajorVersion { get { return Version.Major; } }

        /// <summary>
        /// Returns the minor version of Solid Edge.
        /// </summary>
        public int MinorVersion { get { return Version.Minor; } }

        /// <summary>
        /// Returns the service pack version of Solid Edge.
        /// </summary>
        public int ServicePackVersion { get { return Version.Build; } }

        /// <summary>
        /// Returns the build number of Solid Edge.
        /// </summary>
        public int BuildNumber { get { return Version.Revision; } }
    }
}