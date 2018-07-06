using EyeDenticaService;
using System;
using System.Diagnostics;
using System.IO;
using System.Diagnostics;

/// <summary>
/// This class runs Python code from a file using the console.
/// </summary>
public class PythonScriptRunner
{
    public static void StartRInstances_SeparateProcess(string strFileName)
    {
        // Start the R process
        CreateProcessAsUserWrapper.LaunchChildProcess(strFileName, null);
    }
}