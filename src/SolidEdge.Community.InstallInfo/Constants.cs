using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SolidEdgeCommunity.InstallInfo
{
    internal static class CLSID
    {
        public const string SolidEdge = "DED89DB0-45B6-11CE-B307-0800363A1E02";
    }

    internal static class CATID
    {
        public const string ManagedComponent = "62C8FE65-4EBB-45E7-B440-6E39B2CDBF29";
        public const string SolidEdgeAddIn = "26B1D2D1-2B03-11d2-B589-080036E8B802";
        public const string SEApplication = "26618394-09D6-11d1-BA07-080036230602";
        public const string SEAssembly = "26618395-09D6-11d1-BA07-080036230602";
        public const string SEMotion = "67ED3F40-A351-11d3-A40B-0004AC969602";
        public const string SEPart = "26618396-09D6-11d1-BA07-080036230602";
        public const string SEProfile = "26618397-09D6-11d1-BA07-080036230602";
        public const string SEFeatureRecognition = "E6F9C8DC-B256-11d3-A41E-0004AC969602";
        public const string SESheetMetal = "26618398-09D6-11D1-BA07-080036230602";
        public const string SEDraft = "08244193-B78D-11D2-9216-00C04F79BE98";
        public const string SEWeldment = "7313526A-276F-11D4-B64E-00C04F79B2BF";
        public const string SEXpresRoute = "1661432A-489C-4714-B1B2-61E85CFD0B71";
        public const string SEExplode = "23BE4212-5810-478b-94FF-B4D682C1B538";
        public const string SESimplify = "CE3DCEBF-E36E-4851-930A-ED892FE0772A";
        public const string SEStudio = "D35550BF-0810-4f67-97D5-789EDBC23F4D";
        public const string SELayout = "27B34941-FFCD-4768-9102-0B6698656CEA";
        public const string SESketch = "0DDABC90-125E-4cfe-9CB7-DC97FB74CCF4";
        public const string SEProfileHole = "0D5CC5F7-5BA3-4d2f-B6A9-31D9B401FE30";
        public const string SEProfilePattern = "7BD57D4B-BA47-4a79-A4E2-DFFD43B97ADF";
        public const string SEProfileRevolved = "FB73C683-1536-4073-B792-E28B8D31146E";
        public const string SEDrawingViewEdit = "8DBC3B5F-02D6-4241-BE96-B12EAF83FAE6";
        public const string SERefAxis = "B21CCFF8-1FDD-4f44-9417-F1EAE06888FA";
        public const string SECuttingPlaneLine = "7C6F65F1-A02D-4c3c-8063-8F54B59B34E3";
        public const string SEBrokenOutSectionProfile = "534CAB66-8089-4e18-8FC4-6FA5A957E445";
        public const string SEFrame = "D84119E8-F844-4823-B3A0-D4F31793028A";
        public const string SE2dModel = "F6031120-7D99-48a7-95FC-EEE8038D7996";
        public const string SEEditBlockView = "892A1CDA-12AE-4619-BB69-C5156C929832";
        public const string EditBlockInPlace = "308A1927-CDCE-4b92-B654-241362608CDE";
        public const string SEComponentSketchInPart = "FAB8DC23-00F4-4872-8662-18DD013F2095";
        public const string SEComponentSketchInAsm = "86D925FB-66ED-40d2-AA3D-D04E74838141";
        public const string SEHarness = "5337A0AB-23ED-4261-A238-00E2070406FC";
        public const string SEAll = "C484ED57-DBB6-4a83-BEDB-C08600AF07BF";
        public const string SEAllDocumentEnvrionments = "BAD41B8D-18FF-42c9-9611-8A00E6921AE8";
        public const string SEEditMV = "C1D8CCB8-54D3-4fce-92AB-0668147FC7C3";
        public const string SEEditMVPart = "054BDB42-6C1E-41a4-9014-3D51BEE911EF";
        public const string SEDMPart = "D9B0BB85-3A6C-4086-A0BB-88A1AAD57A58";
        public const string SEDMSheetMetal = "9CBF2809-FF80-4dbc-98F2-B82DABF3530F";
        public const string SEDMAssembly = "2C3C2A72-3A4A-471d-98B5-E3A8CFA4A2BF";
        public const string FEAResultsPart = "B5965D1C-8819-4902-8252-64841537A16C";
        public const string FEAResultsAsm = "986B2512-3AE9-4a57-8513-1D2A1E3520DD";
        public const string SESimplifiedAssemblyPart = "E7350DC3-6E7A-4D53-A53F-5B1C7A0709B3";
        public const string Sketch3d = "07F05BA4-18CD-4B87-8E2F-49864E71B41F";
        public const string SEAssemblyViewer = "F2483121-58BC-44AF-8B8F-D7B74DC8408B";
    }

    public static class KnownCategoryIds
    {
        private const string SECATIDS_H = "secatids.h";

        public static string GetDescription(Guid guid)
        {
            var type = typeof(KnownCategoryIds);
            var field = type.GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(x => x.FieldType == typeof(Guid))
                .FirstOrDefault(x => object.Equals(x.GetValue(null), guid));

            var descriptionAttributes = field?.GetCustomAttributes(typeof(DescriptionAttribute), false);
            var descriptionAttribute = descriptionAttributes?.OfType<DescriptionAttribute>().FirstOrDefault();

            return $"{descriptionAttribute?.Description}";
        }

        public static string GetNativeSymbolicName(Guid guid)
        {
            var type = typeof(KnownCategoryIds);
            var field = type.GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(x => x.FieldType == typeof(Guid))
                .FirstOrDefault(x => object.Equals(x.GetValue(null), guid));

            var symbolicNameAttributes = field?.GetCustomAttributes(typeof(NativeSymbolicNameAttributeAttribute), false);
            var symbolicNameAttribute = symbolicNameAttributes?.OfType<NativeSymbolicNameAttributeAttribute>().FirstOrDefault();

            return $"{symbolicNameAttribute?.Name}";
        }

        public static string GetNativeInclude(Guid guid)
        {
            var type = typeof(KnownCategoryIds);
            var field = type.GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(x => x.FieldType == typeof(Guid))
                .FirstOrDefault(x => object.Equals(x.GetValue(null), guid));

            var symbolicNameAttributes = field?.GetCustomAttributes(typeof(NativeSymbolicNameAttributeAttribute), false);
            var symbolicNameAttribute = symbolicNameAttributes?.OfType<NativeSymbolicNameAttributeAttribute>().FirstOrDefault();

            return $"{symbolicNameAttribute?.Include}";
        }

        [Description(".NET Category")]
        public static readonly Guid ManagedComponent = new Guid(CATID.ManagedComponent);

        [Description("Solid Edge AddIn")]
        [NativeSymbolicNameAttribute("CATID_SolidEdgeAddIn", SECATIDS_H)]
        public static readonly Guid SolidEdgeAddIn = new Guid(CATID.SolidEdgeAddIn);

        [Description("Solid Edge Application Environment")]
        [NativeSymbolicNameAttribute("CATID_SEApplication", SECATIDS_H)]
        public static readonly Guid SEApplication = new Guid(CATID.SEApplication);

        [Description("Solid Edge Assembly Environment")]
        [NativeSymbolicNameAttribute("CATID_SEAssembly", SECATIDS_H)]
        public static readonly Guid SEAssembly = new Guid(CATID.SEAssembly);

        [Description("Solid Edge Motion Environment")]
        [NativeSymbolicNameAttribute("CATID_SEMotion", SECATIDS_H)]
        public static readonly Guid SEMotion = new Guid(CATID.SEMotion);

        [Description("Solid Edge Part Environment")]
        [NativeSymbolicNameAttribute("CATID_SEPart", SECATIDS_H)]
        public static readonly Guid SEPart = new Guid(CATID.SEPart);

        [Description("Solid Edge Profile Environment")]
        [NativeSymbolicNameAttribute("CATID_SEProfile", SECATIDS_H)]
        public static readonly Guid SEProfile = new Guid(CATID.SEProfile);

        [Description("Solid Edge Assembly Environment")]
        [NativeSymbolicNameAttribute("CATID_SEFeatureRecognition", SECATIDS_H)]
        public static readonly Guid SEFeatureRecognition = new Guid(CATID.SEFeatureRecognition);

        [Description("Solid Edge SheetMetal Environment")]
        [NativeSymbolicNameAttribute("CATID_SESheetMetal", SECATIDS_H)]
        public static readonly Guid SESheetMetal = new Guid(CATID.SESheetMetal);

        [Description("Solid Edge Draft Environment")]
        [NativeSymbolicNameAttribute("CATID_SEDraft", SECATIDS_H)]
        public static readonly Guid SEDraft = new Guid(CATID.SEDraft);

        [Description("Solid Edge Weldment Environment")]
        [NativeSymbolicNameAttribute("CATID_SEWeldment", SECATIDS_H)]
        public static readonly Guid SEWeldment = new Guid(CATID.SEWeldment);

        [Description("Solid Edge XpresRoute Environment")]
        [NativeSymbolicNameAttribute("CATID_SEXpresRoute", SECATIDS_H)]
        public static readonly Guid SEXpresRoute = new Guid(CATID.SEXpresRoute);

        [Description("Solid Edge Explode Environment")]
        [NativeSymbolicNameAttribute("CATID_SEExplode", SECATIDS_H)]
        public static readonly Guid SEExplode = new Guid(CATID.SEExplode);

        [Description("Solid Edge Simplify Environment")]
        [NativeSymbolicNameAttribute("CATID_SESimplify", SECATIDS_H)]
        public static readonly Guid SESimplify = new Guid(CATID.SESimplify);

        [Description("Solid Edge Studio Environment")]
        [NativeSymbolicNameAttribute("CATID_SEStudio", SECATIDS_H)]
        public static readonly Guid SEStudio = new Guid(CATID.SEStudio);

        [Description("Solid Edge Layout Environment")]
        [NativeSymbolicNameAttribute("CATID_SELayout", SECATIDS_H)]
        public static readonly Guid SELayout = new Guid(CATID.SELayout);

        [Description("Solid Edge Sketch Environment")]
        [NativeSymbolicNameAttribute("CATID_SESketch", SECATIDS_H)]
        public static readonly Guid SESketch = new Guid(CATID.SESketch);

        [Description("Solid Edge Profile Hole Environment")]
        [NativeSymbolicNameAttribute("CATID_SEProfileHole", SECATIDS_H)]
        public static readonly Guid SEProfileHole = new Guid(CATID.SEProfileHole);

        [Description("Solid Edge Profile Pattern Environment")]
        [NativeSymbolicNameAttribute("CATID_SEProfilePattern", SECATIDS_H)]
        public static readonly Guid SEProfilePattern = new Guid(CATID.SEProfilePattern);

        [Description("Solid Edge Profile Revolved Environment")]
        [NativeSymbolicNameAttribute("CATID_SEProfileRevolved", SECATIDS_H)]
        public static readonly Guid SEProfileRevolved = new Guid(CATID.SEProfileRevolved);

        [Description("Solid Edge Drawing View Edit Environment")]
        [NativeSymbolicNameAttribute("CATID_SEDrawingViewEdit", SECATIDS_H)]
        public static readonly Guid SEDrawingViewEdit = new Guid(CATID.SEDrawingViewEdit);

        [Description("Solid Edge Ref Axis Environment")]
        [NativeSymbolicNameAttribute("CATID_SERefAxis", SECATIDS_H)]
        public static readonly Guid SERefAxis = new Guid(CATID.SERefAxis);

        [Description("Solid Edge Cutting Plane Environment")]
        [NativeSymbolicNameAttribute("CATID_SECuttingPlaneLine", SECATIDS_H)]
        public static readonly Guid SECuttingPlaneLine = new Guid(CATID.SECuttingPlaneLine);

        [Description("Solid Edge Broken Out Section Profile Environment")]
        [NativeSymbolicNameAttribute("CATID_SEBrokenOutSectionProfile", SECATIDS_H)]
        public static readonly Guid SEBrokenOutSectionProfile = new Guid(CATID.SEBrokenOutSectionProfile);

        [Description("Solid Edge Frame Environment")]
        [NativeSymbolicNameAttribute("CATID_SEFrame", SECATIDS_H)]
        public static readonly Guid SEFrame = new Guid(CATID.SEFrame);

        [Description("Solid Edge 2D Model Environment")]
        [NativeSymbolicNameAttribute("CATID_SE2dModel", SECATIDS_H)]
        public static readonly Guid SE2dModel = new Guid(CATID.SE2dModel);

        [Description("Solid Edge Edit Block View Environment")]
        [NativeSymbolicNameAttribute("CATID_SEEditBlockView", SECATIDS_H)]
        public static readonly Guid SEEditBlockView = new Guid(CATID.SEEditBlockView);

        [Description("Solid Edge Component Sketch In Part Environment")]
        [NativeSymbolicNameAttribute("CATID_SEComponentSketchInPart", SECATIDS_H)]
        public static readonly Guid SEComponentSketchInPart = new Guid(CATID.SEComponentSketchInPart);

        [Description("Solid Edge Component Sketch Is Assembly Environment")]
        [NativeSymbolicNameAttribute("CATID_SEComponentSketchInAsm", SECATIDS_H)]
        public static readonly Guid SEComponentSketchInAsm = new Guid(CATID.SEComponentSketchInAsm);

        [Description("Solid Edge Harness Environment")]
        [NativeSymbolicNameAttribute("CATID_SEHarness", SECATIDS_H)]
        public static readonly Guid SEHarness = new Guid(CATID.SEHarness);

        [Description("Solid Edge All Environments")]
        [NativeSymbolicNameAttribute("CATID_SEAll", SECATIDS_H)]
        public static readonly Guid SEAll = new Guid(CATID.SEAll);

        [Description("Solid Edge All Document Environments")]
        [NativeSymbolicNameAttribute("CATID_SEAllDocumentEnvrionments", SECATIDS_H)]
        public static readonly Guid SEAllDocumentEnvrionments = new Guid(CATID.SEAllDocumentEnvrionments);

        [Description("Solid Edge Synchronous Part Environment")]
        [NativeSymbolicNameAttribute("CATID_SEDMPart", SECATIDS_H)]
        public static readonly Guid SEDMPart = new Guid(CATID.SEDMPart);

        [Description("Solid Edge Synchronous SheetMetal Environment")]
        [NativeSymbolicNameAttribute("CATID_SEDMSheetMetal", SECATIDS_H)]
        public static readonly Guid SEDMSheetMetal = new Guid(CATID.SEDMSheetMetal);

        [Description("Solid Edge Synchronous Asssembly Environment")]
        [NativeSymbolicNameAttribute("CATID_SEDMAssembly", SECATIDS_H)]
        public static readonly Guid SEDMAssembly = new Guid(CATID.SEDMAssembly);

        [Description("Solid Edge Simplified Assembly Part Environment")]
        [NativeSymbolicNameAttribute("CATID_SESimplifiedAssemblyPart", SECATIDS_H)]
        public static readonly Guid SESimplifiedAssemblyPart = new Guid(CATID.SESimplifiedAssemblyPart);

        [Description("Solid Edge Sketch 3D Environment")]
        [NativeSymbolicNameAttribute("CATID_Sketch3d", SECATIDS_H)]
        public static readonly Guid Sketch3d = new Guid(CATID.Sketch3d);

        [Description("Solid Edge Edit Block In Place Environment")]
        [NativeSymbolicNameAttribute("CATID_EditBlockInPlace", SECATIDS_H)]
        public static readonly Guid EditBlockInPlace = new Guid(CATID.EditBlockInPlace);

        [Description("Solid Edge Edit MV Environment")]
        [NativeSymbolicNameAttribute("CATID_SEEditMV", SECATIDS_H)]
        public static readonly Guid SEEditMV = new Guid(CATID.SEEditMV);

        [Description("Solid Edge Edit MV Part Environment")]
        [NativeSymbolicNameAttribute("CATID_SEEditMVPart", SECATIDS_H)]
        public static readonly Guid SEEditMVPart = new Guid(CATID.SEEditMVPart);

        [Description("Solid Edge FEA Results Part Environment")]
        [NativeSymbolicNameAttribute("CATID_FEAResultsPart", SECATIDS_H)]
        public static readonly Guid FEAResultsPart = new Guid(CATID.FEAResultsPart);

        [Description("Solid Edge FEA Results Assembly Environment")]
        [NativeSymbolicNameAttribute("CATID_FEAResultsAsm", SECATIDS_H)]
        public static readonly Guid FEAResultsAsm = new Guid(CATID.FEAResultsAsm);

        [Description("Solid Edge Assembly Viewer Environment")]
        [NativeSymbolicNameAttribute("CATID_SEAssemblyViewer", SECATIDS_H)]
        public static readonly Guid SEAssemblyViewer = new Guid(CATID.SEAssemblyViewer);
    }
}
