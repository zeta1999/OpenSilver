﻿using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.IO;

namespace DotNetForHtml5.Compiler
{
    //[LoadInSeparateAppDomain]
    //[Serializable]
    public class AssemblyAttributesGenerator : Task // AppDomainIsolatedTask
    {
        [Required]
        public bool IsSLMigration { get; set; }

        [Required]
        public string OutputFile { get; set; }

        [Required]
        public string IntermediateOutputAbsolutePath { get; set; }

#if !BRIDGE && !CSHTML5BLAZOR
        [Required]
        public string OutputRootPath { get; set; }

        [Required]
        public string OutputAppFilesPath { get; set; }

        [Required]
        public string OutputLibrariesPath { get; set; }

        [Required]
        public string OutputResourcesPath { get; set; }

        [Required]
        public bool GenerateJavaScriptDuringBuild { get; set; }
#endif

        public override bool Execute()
        {
            ILogger logger = new LoggerThatUsesTaskOutput(this);
            string operationName = "C#/XAML for HTML5: AssemblyAttributesGenerator";
            try
            {
                // Validate input strings:
                if (string.IsNullOrEmpty(OutputFile))
                    throw new Exception(operationName + " failed because the output file argument is invalid.");

                //------- DISPLAY THE PROGRESS -------
                logger.WriteMessage(operationName + " started.");

                // Generate attributes:
#if BRIDGE || CSHTML5BLAZOR
                string generatedCode = string.Format(@"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by ""CSHTML5""
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
[assembly: global::CSHTML5.Internal.Attributes.CompilerVersionNumber(""{0}"")]
[assembly: global::CSHTML5.Internal.Attributes.CompilerVersionFriendlyName(""{1}"")]
[assembly: global::CSHTML5.Internal.Attributes.CompilerIsSLMigrationAttribute({2})]
[assembly: global::CSHTML5.Internal.Attributes.IntermediateOutputAbsolutePath(@""{3}"")]
[assembly: global::CSHTML5.Internal.Attributes.SettingsAttribute()]
",
                    VersionInformation.GetCurrentVersionNumber().ToString(),
                    VersionInformation.GetCurrentVersionFriendlyName(),
                    (this.IsSLMigration ? "true" : "false"),
                    IntermediateOutputAbsolutePath
                );
#else
                string generatedCode = string.Format(@"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by ""C#/XAML for HTML5""
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::CompilerVersionNumber(""{0}"")]
[assembly: global::CompilerVersionFriendlyName(""{1}"")]
[assembly: global::CompilerIsSLMigration({2})]

[assembly: global::CSHTML5.Internal.Attributes.OutputRootPath(@""{3}"")]
[assembly: global::CSHTML5.Internal.Attributes.OutputAppFilesPath(@""{4}"")]
[assembly: global::CSHTML5.Internal.Attributes.OutputLibrariesPath(@""{5}"")]
[assembly: global::CSHTML5.Internal.Attributes.OutputResourcesPath(@""{6}"")]
[assembly: global::CSHTML5.Internal.Attributes.IntermediateOutputAbsolutePath(@""{7}"")]

[assembly: global::CSHTML5.Internal.Attributes.SettingsAttribute(GenerateJavaScriptDuringBuild = {8})]
"
                    ,
                    VersionInformation.GetCurrentVersionNumber().ToString(),
                    VersionInformation.GetCurrentVersionFriendlyName(),
                    (this.IsSLMigration ? "true" : "false"),
                    OutputRootPath,
                    OutputAppFilesPath,
                    OutputLibrariesPath,
                    OutputResourcesPath,
                    IntermediateOutputAbsolutePath,
                    (this.GenerateJavaScriptDuringBuild ? "true" : "false")
                );
#endif
                // Create output directory:
                Directory.CreateDirectory(Path.GetDirectoryName(OutputFile));

                // Save output:
                using (StreamWriter outfile = new StreamWriter(OutputFile))
                {
                    outfile.Write(generatedCode);
                }

                //------- DISPLAY THE PROGRESS -------
                logger.WriteMessage(operationName + " completed.");

                return true;
            }
            catch (Exception ex)
            {
                logger.WriteError(operationName + " failed: " + ex.ToString());
                return false;
            }
        }
    }
}
