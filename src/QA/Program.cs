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
    }
}
