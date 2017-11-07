using SolidEdgeCommunity.InstallInfo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QA
{
    class Program
    {
        static void Main(string[] args)
        {
            //System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture
            
            var os = Environment.OSVersion;
            TestProductCodesFromMsi();

            TestInstallInfo();
            TestAddIns();
            TestProductInfo();
            
        }

        static void TestInstallInfo()
        {
            var all = SolidEdgeInstallInfo.All;
            var installInfo = SolidEdgeInstallInfo.Default;
        }

        static void TestAddIns()
        {
            var addins = SolidEdgeAddInInfo.All;

            foreach (var addin in addins)
            {
                foreach (var environmentCategory in addin.EnvironmentCategories)
                {
                }
            }
        }

        static void TestProductInfo()
        {
            var allKnown = SolidEdgeProductInfo.AllKnown;

            foreach (var productInfo in allKnown)
            {
            }
        }

        static void TestProductCodesFromMsi()
        {
            var msiPaths = new string[]
                {
                    @"D:\Software\Solid Edge\ST\SolidEdgeV100_32bit_ENGLISH\Solid Edge\Solid Edge ST.msi",
                    @"D:\Software\Solid Edge\ST\SolidEdgeV100_64bit_ENGLISH\Solid Edge\Solid Edge ST.msi",
                    @"D:\Software\Solid Edge\ST2\SolidEdgeV102_32bit_ENGLISH\Solid Edge\Solid Edge ST2.msi",
                    @"D:\Software\Solid Edge\ST2\SolidEdgeV102_64bit_ENGLISH\Solid Edge\Solid Edge ST2.msi",
                    @"D:\Software\Solid Edge\ST3\SolidEdgeV103_32bit_ENGLISH\Solid Edge\Solid Edge ST3.msi",
                    @"D:\Software\Solid Edge\ST3\SolidEdgeV103_64bit_ENGLISH\Solid Edge\Solid Edge ST3.msi",
                    @"D:\Software\Solid Edge\ST4\SolidEdgeV104ENGLISH_32Bit\CDROM\Solid Edge\Solid Edge ST4.msi",
                    @"D:\Software\Solid Edge\ST4\SolidEdgeV104ENGLISH_64Bit\CDROM\Solid Edge\Solid Edge ST4.msi",
                    @"D:\Software\Solid Edge\ST5\SolidEdgeV105ENGLISH_32Bit\DVD\Solid Edge\Solid Edge ST5.msi",
                    @"D:\Software\Solid Edge\ST5\SolidEdgeV105ENGLISH_64Bit\DVD\Solid Edge\Solid Edge ST5.msi",
                    @"D:\Software\Solid Edge\ST6\SolidEdgeV106ENGLISH_32Bit\DVD\Solid Edge\Solid Edge ST6.msi",
                    @"D:\Software\Solid Edge\ST6\SolidEdgeV106ENGLISH_64Bit\DVD\Solid Edge\Solid Edge ST6.msi",
                    @"D:\Software\Solid Edge\ST7\Solid_Edge_DVD_ENGLISH_ST7\DVD\Solid Edge\Solid Edge ST7.msi",
                    @"D:\Software\Solid Edge\ST8\Solid_Edge_DVD_ENGLISH_ST8\DVD\Solid Edge\Solid Edge ST8.msi",
                    @"D:\Software\Solid Edge\ST9\Solid_Edge_DVD_ENGLISH_ST9\DVD\Solid Edge\Solid Edge ST9.msi",
                    @"D:\Software\Solid Edge\ST10\Solid_Edge_DVD_ENGLISH__ST10\DVD\Solid Edge\Solid Edge ST10.msi"
                };

            foreach (var msiPath in msiPaths)
            {
                Console.WriteLine($"{msiPath}");
                var foo = DumpMsiProperties(msiPath);
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                //var productCode = GetProductCodeFromMsi(msiPath);

                //Console.WriteLine($"{msiPath} - {productCode}");
            }
        }

        static string GetProductCodeFromMsi(string msiPath)
        {
            var type = Type.GetTypeFromProgID("WindowsInstaller.Installer");
            dynamic installer = Activator.CreateInstance(type);
            dynamic database = installer.OpenDatabase(msiPath, 0);
            dynamic view = database.OpenView("SELECT * FROM Property WHERE Property = 'ProductCode'");
            view.Execute(null);
            dynamic r = view.Fetch();
            string version = r.StringData[2];
            return version;
        }

        static string DumpMsiProperties(string msiPath)
        {
            var type = Type.GetTypeFromProgID("WindowsInstaller.Installer");
            var installer = (WindowsInstaller.Installer)Activator.CreateInstance(type);
            var database = installer.OpenDatabase(msiPath, 0);

            var view = database.OpenView("SELECT * FROM Property");

            view.Execute(null);

            var record = view.Fetch();

            while (record != null)
            {
                //var fieldCount = record.FieldCount;

                //for (int i = 0; i <= fieldCount; i++)
                //{
                //    var dataSize = record.DataSize[i];
                //    var stringData = record.StringData[i];
                //}


                Console.WriteLine($"{record.StringData[1]}={record.StringData[2]}");
                record = view.Fetch();
            }

            return null;
        }
    }
}
