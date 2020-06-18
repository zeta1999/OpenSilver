﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DotNetForHtml5.Compiler
{
    internal static class ShortPathHelper
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int GetShortPathName(String pathName, StringBuilder shortName, int cbShortName);

        public static string GetShortPathName(string fullAbsolutePath)
        {
            StringBuilder sb = new StringBuilder(300);
            int n = GetShortPathName(fullAbsolutePath, sb, 300);
            if (n == 0) // check for errors
            {
                int errorCode = Marshal.GetLastWin32Error();
                string pleaseReportThisError = "   - Please report this error to support@cshtml5.com";
                if (errorCode == 2)
                    throw new Exception("The following folder was not found: " + fullAbsolutePath + pleaseReportThisError);
                else
                    throw new Exception("GetShortPathName failed with error code " + errorCode.ToString() + pleaseReportThisError);
            }
            else
                return sb.ToString();
        }
    }
}
