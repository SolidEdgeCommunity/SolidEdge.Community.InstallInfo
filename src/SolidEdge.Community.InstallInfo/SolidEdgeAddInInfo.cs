using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SolidEdgeCommunity.InstallInfo
{

    /// <summary>
    /// Provides information about registered Solid Edge AddIns.
    /// </summary>
    public abstract class SolidEdgeAddInInfo
    {
        static SolidEdgeAddInInfo()
        {
            Refresh();
        }

        internal SolidEdgeAddInInfo()
        {
            // Initialize arrays.
            Descriptions = new CultureString[] { };
            Summaries = new CultureString[] { };
            ImplementedCategories = new ComponentCategoryInfo[] { };
            EnvironmentCategories = new ComponentCategoryInfo[] { };
        }

        /// <summary>
        /// Refreshes cached data.
        /// </summary>
        public static void Refresh()
        {
            var all = new List<SolidEdgeAddInInfo>();

            if (Environment.Is64BitOperatingSystem)
            {
                all.AddRange(ProcessClassesRootByRegistryView(RegistryView.Registry64));
                all.AddRange(ProcessClassesRootByRegistryView(RegistryView.Registry32));
            }
            else
            {
                all.AddRange(ProcessClassesRootByRegistryView(RegistryView.Registry32));
            }

            All = all.ToArray();
        }

        static IEnumerable<SolidEdgeAddInInfo> ProcessClassesRootByRegistryView(RegistryView registryView)
        {
            var addinsCLSIDs = GetCLSIDsThatImplementSolidEdgeAddIn(registryView);

            var list = new List<SolidEdgeAddInInfo>();

            using (var localMachineKey = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, registryView))
            {
                foreach (var addinsCLSID in addinsCLSIDs)
                {
                    using (var clsidAddInKey = localMachineKey.OpenSubKey($@"CLSID\{addinsCLSID.ToStringEnclosedWithBraces()}", false))
                    {
                        if (clsidAddInKey != null)
                        {
                            var environmentCategories = GetAddInEnvironmentCategories(clsidAddInKey);
                            var implementedCategories = GetAddInImplementedCategories(clsidAddInKey);

                            var autoConnect = (int)clsidAddInKey.GetValue("AutoConnect", 0);
                            var isManagedComponent = implementedCategories.FirstOrDefault(x => x.CATID == KnownCategoryIds.ManagedComponent) != null;
                            
                            var addInInfo = default(SolidEdgeAddInInfo);

                            if (isManagedComponent)
                            {
                                addInInfo = new ManagedSolidEdgeAddInInfo();
                            }
                            else
                            {
                                addInInfo = new NativeSolidEdgeAddInInfo();
                            }

                            addInInfo.AutoConnect = autoConnect == 0 ? false : true;
                            addInInfo.CLSID = addinsCLSID;
                            addInInfo.EnvironmentCategories = environmentCategories;
                            addInInfo.ImplementedCategories = implementedCategories.ToArray();
                            addInInfo.RegistryPath = clsidAddInKey.ToString();

                            switch (clsidAddInKey.View)
                            {
                                case RegistryView.Registry32:
                                    addInInfo.Platform = "x86";
                                    break;
                                case RegistryView.Registry64:
                                    addInInfo.Platform = "x64";
                                    break;
                            }

                            addInInfo.Descriptions = GetCultureStringsFromKeyValues(clsidAddInKey).ToArray();

                            using (var summaryKey = clsidAddInKey.OpenSubKey("Summary", false))
                            {
                                if (summaryKey != null)
                                {
                                    addInInfo.Summaries = GetCultureStringsFromKeyValues(summaryKey).ToArray();
                                }
                            }

                            ProcessInprocServer32(clsidAddInKey, addInInfo);

                            list.Add(addInInfo);
                        }
                    }
                }
            }

            return list;
        }

        static Guid[] GetCLSIDsThatImplementSolidEdgeAddIn(RegistryView registryView)
        {
            var list = new List<Guid>();

            using (var localMachineKey = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, registryView))
            {
                using (var clsidRootKey = localMachineKey.OpenSubKey("CLSID", false))
                {
                    if (clsidRootKey != null)
                    {
                        foreach (var clsidKeyName in clsidRootKey.GetSubKeyNames())
                        {
                            using (var clsidKey = clsidRootKey.OpenSubKey(clsidKeyName, false))
                            {
                                using (var implementedCategoryKey = clsidKey.OpenSubKey($@"Implemented Categories\{KnownCategoryIds.SolidEdgeAddIn.ToStringEnclosedWithBraces()}", false))
                                {
                                    if (implementedCategoryKey != null)
                                    {
                                        if (Guid.TryParse(clsidKeyName, out Guid result))
                                        {
                                            list.Add(result);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return list.ToArray();
        }

        static ComponentCategoryInfo[] GetAddInImplementedCategories(RegistryKey clsidAddInKey)
        {
            var list = new List<ComponentCategoryInfo>();

            using (var implementedCategoriesKey = clsidAddInKey.OpenSubKey("Implemented Categories", false))
            {
                if (implementedCategoriesKey != null)
                {
                    var subKeyNames = implementedCategoriesKey.GetSubKeyNames();

                    foreach (var subKeyName in subKeyNames)
                    {
                        if (Guid.TryParse(subKeyName, out Guid result))
                        {
                            var description = KnownCategoryIds.GetDescription(result);
                            var nativeInclude = KnownCategoryIds.GetNativeInclude(result);
                            var natvieSymbolicName = KnownCategoryIds.GetNativeSymbolicName(result);

                            list.Add(new ComponentCategoryInfo()
                            {
                                CATID = result,
                                Description = description,
                                NativeInclude = nativeInclude,
                                NativeSymbolicName = natvieSymbolicName
                            });
                        }
                    }
                }
            }

            return list.ToArray();
        }

        static ComponentCategoryInfo[] GetAddInEnvironmentCategories(RegistryKey clsidAddInKey)
        {
            var list = new List<ComponentCategoryInfo>();

            using (var implementedCategoriesKey = clsidAddInKey.OpenSubKey("Environment Categories", false))
            {
                if (implementedCategoriesKey != null)
                {
                    var subKeyNames = implementedCategoriesKey.GetSubKeyNames();

                    foreach (var subKeyName in subKeyNames)
                    {
                        if (Guid.TryParse(subKeyName, out Guid result))
                        {
                            var description = KnownCategoryIds.GetDescription(result);
                            var nativeInclude = KnownCategoryIds.GetNativeInclude(result);
                            var symbolicName = KnownCategoryIds.GetNativeSymbolicName(result);

                            list.Add(new ComponentCategoryInfo()
                            {
                                CATID = result,
                                Description = description,
                                NativeInclude = nativeInclude,
                                NativeSymbolicName = symbolicName
                            });
                        }
                    }
                }
            }

            return list.ToArray();
        }

        static IEnumerable<CultureString> GetCultureStringsFromKeyValues(RegistryKey registryKey)
        {
            var cultureStrings = new List<CultureString>();

            var cultures = CultureInfo.GetCultures(CultureTypes.InstalledWin32Cultures);
            var valueNames = registryKey.GetValueNames();

            foreach (var valueName in valueNames)
            {
                if (registryKey.GetValueKind(valueName) == RegistryValueKind.String)
                {
                    if (int.TryParse(valueName, out int hexLCID))
                    {
                        int intLCID = int.Parse(hexLCID.ToString(), System.Globalization.NumberStyles.HexNumber);

                        var culture = cultures.FirstOrDefault(x => x.LCID == intLCID);

                        if (culture != null)
                        {
                            var description = $"{registryKey.GetValue(valueName, null)}";
                            cultureStrings.Add(new CultureString(culture, description));
                        }
                    }
                }
            }

            return cultureStrings;
        }

        static void ProcessInprocServer32(RegistryKey clsidAddInKey, SolidEdgeAddInInfo addInInfo)
        {
            if (addInInfo is ManagedSolidEdgeAddInInfo)
            {
                ProcessInprocServer32(clsidAddInKey, (ManagedSolidEdgeAddInInfo)addInInfo);
            }
            else if (addInInfo is NativeSolidEdgeAddInInfo)
            {
                ProcessInprocServer32(clsidAddInKey, (NativeSolidEdgeAddInInfo)addInInfo);
            }
        }

        static void ProcessInprocServer32(RegistryKey clsidAddInKey, ManagedSolidEdgeAddInInfo addInInfo)
        {
            using (var implementedCategoriesKey = clsidAddInKey.OpenSubKey("InprocServer32", false))
            {
                if (implementedCategoriesKey != null)
                {
                    addInInfo.Assembly = $"{implementedCategoriesKey.GetValue("Assembly", null)}";
                    addInInfo.Class = $"{implementedCategoriesKey.GetValue("Class", null)}";
                    addInInfo.CodeBase = $"{implementedCategoriesKey.GetValue("CodeBase", null)}";
                    addInInfo.HostDll = $"{implementedCategoriesKey.GetValue(null, null)}";
                    addInInfo.RuntimeVersion = $"{implementedCategoriesKey.GetValue("RuntimeVersion", null)}";
                    addInInfo.ThreadingModel = $"{implementedCategoriesKey.GetValue("ThreadingModel", null)}";
                }
            }
        }

        static void ProcessInprocServer32(RegistryKey clsidAddInKey, NativeSolidEdgeAddInInfo addInInfo)
        {
            using (var implementedCategoriesKey = clsidAddInKey.OpenSubKey("InprocServer32", false))
            {
                if (implementedCategoriesKey != null)
                {
                    addInInfo.HostDll = $"{implementedCategoriesKey.GetValue(null, null)}";
                    addInInfo.ThreadingModel = $"{implementedCategoriesKey.GetValue("ThreadingModel", null)}";
                }
            }
        }

        public override string ToString()
        {
            return $"{Platform} | {Description} | {Summary}";
        }

        /// <summary>
        /// Returns an array of objects of type <see cref="SolidEdgeCommunity.InstallInfo.SolidEdgeAddInInfo"/>.
        /// </summary>
        public static SolidEdgeAddInInfo[] All { get; private set; }

        /// <summary>
        /// Returns an array of objects of type <see cref="SolidEdgeCommunity.InstallInfo.ManagedSolidEdgeAddInInfo"/>.
        /// </summary>
        public static ManagedSolidEdgeAddInInfo[] ManagedAddIns { get { return All.OfType<ManagedSolidEdgeAddInInfo>().ToArray(); } }

        /// <summary>
        /// Returns an array of objects of type <see cref="SolidEdgeCommunity.InstallInfo.NativeSolidEdgeAddInInfo"/>.
        /// </summary>
        public static NativeSolidEdgeAddInInfo[] NativeAddIns { get { return All.OfType<NativeSolidEdgeAddInInfo>().ToArray(); } }

        /// <summary>
        /// Returns the full registry path of the component.
        /// </summary>
        public string RegistryPath { get; private set; }

        /// <summary>
        /// Returns the platform of the component.
        /// </summary>
        /// <remarks>
        /// Components registered in the 64 bit view of the registry will return 'x64'. Components registered in the 32 bit view of the registry will return 'x86'.
        /// </remarks>
        public string Platform { get; private set; }

        /// <summary>
        /// Returns true if the Solid Edge AddIn is enabled.
        /// </summary>
        public bool AutoConnect { get; private set; }

        /// <summary>
        /// Returns an array of <see cref="SolidEdgeCommunity.InstallInfo.CultureString"/> objects. Each object represents a culture specific description.
        /// </summary>
        public CultureString[] Descriptions { get; private set; }

        /// <summary>
        /// Returns an array of <see cref="SolidEdgeCommunity.InstallInfo.CultureString"/> objects. Each object represents a culture specific summary.
        /// </summary>
        public CultureString[] Summaries { get; private set; }

        /// <summary>
        /// Returns the CLSID of the component.
        /// </summary>
        public Guid CLSID { get; private set; }

        /// <summary>
        /// Returns an array of <see cref="SolidEdgeCommunity.InstallInfo.ComponentCategoryInfo"/> objects. Each object represents the category that the component is registered to.
        /// </summary>
        public ComponentCategoryInfo[] ImplementedCategories { get; private set; }

        /// <summary>
        /// Returns an array of <see cref="SolidEdgeCommunity.InstallInfo.ComponentCategoryInfo"/> objects. Each object represents the Solid Edge environment that the addin is registered to.
        /// </summary>
        public ComponentCategoryInfo[] EnvironmentCategories { get; private set; }

        /// <summary>
        /// Returns the threading model of the component.
        /// </summary>
        public string ThreadingModel { get; private set; }

        /// <summary>
        /// Returns the host DLL of the component.
        /// </summary>
        public string HostDll { get; private set; }

        /// <summary>
        /// Returns the description matching the OS culture. If no match is found, returns the first description.
        /// </summary>
        public string Description
        {
            get
            {
                var localDescription = Descriptions.FirstOrDefault(x => x.Culture == CultureInfo.InstalledUICulture);
                var firstDescription = Descriptions.FirstOrDefault();

                return localDescription == null ? firstDescription?.Value : localDescription?.Value;
            }
        }

        /// <summary>
        /// Returns the summary matching the OS culture. If no match is found, returns the first summary.
        /// </summary>
        public string Summary
        {
            get
            {
                var localSummary = Summaries.FirstOrDefault(x => x.Culture == CultureInfo.InstalledUICulture);
                var firstSummary = Summaries.FirstOrDefault();

                return localSummary == null ? firstSummary?.Value : localSummary?.Value;
            }
        }
    }

    /// <summary>
    /// Provides information about registered native Solid Edge AddIns.
    /// </summary>
    public class NativeSolidEdgeAddInInfo : SolidEdgeAddInInfo
    {
        internal NativeSolidEdgeAddInInfo()
            : base()
        {
        }

        public static new NativeSolidEdgeAddInInfo[] All
        {
            get
            {
                return SolidEdgeAddInInfo.All.OfType<NativeSolidEdgeAddInInfo>().ToArray();
            }
        }
    }

    /// <summary>
    /// Provides information about registered managed Solid Edge AddIns.
    /// </summary>
    public class ManagedSolidEdgeAddInInfo : SolidEdgeAddInInfo
    {
        internal ManagedSolidEdgeAddInInfo()
            : base()
        {
        }

        public static new ManagedSolidEdgeAddInInfo[] All
        {
            get
            {
                return SolidEdgeAddInInfo.All.OfType<ManagedSolidEdgeAddInInfo>().ToArray();
            }
        }

        /// <summary>
        /// Returns the assembly name.
        /// </summary>
        public string Assembly { get; internal set; }

        /// <summary>
        /// Returns the class that was registered as a COM component in the assembly.
        /// </summary>
        public string Class { get; internal set; }

        /// <summary>
        /// Returns the code base of the assembly.
        /// </summary>
        /// <remarks>
        /// Only available if specified during assembly registration.
        /// </remarks>
        public string CodeBase { get; internal set; }

        /// <summary>
        /// Returns the runtime version of the assembly.
        /// </summary>
        public string RuntimeVersion { get; internal set; }
    }
}
