using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TDF.Demo.AdminWeb.Models.OCR.Tools
{
   public class FceConfig
    {
        // full path to FCE dll
        public const string DllPath = "D:\\Program Files (x86)\\ABBYY SDK\\11\\FlexiCapture Engine\\Bin\\FCEngine.dll";

        // Return full path to FCE dll
        public static string GetDllPath()
        {
            return "D:\\Program Files (x86)\\ABBYY SDK\\11\\FlexiCapture Engine\\Bin\\FCEngine.dll";
        }

        // Return developer serial number for FCE
        public static string GetDeveloperSN()
        {
            return "SWTT11020006284214167950";
        }

        // Return full path to Samples directory
        public static string GetSamplesFolder()
        {
            return "C:\\ProgramData\\ABBYY\\SDK\\11\\FlexiCapture Engine\\Samples";
        }

    }
}