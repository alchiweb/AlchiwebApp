using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.Invocation;
using System.Reflection;
using System.Text;

namespace AlchiwebApp.Console.AddToBitPlatform;

internal class CustomVersionAction : SynchronousCommandLineAction
{

    public override int Invoke(ParseResult parseResult)
    {
        var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;

        System.Console.WriteLine($"Version: {assemblyVersion} / Tested with BitPlatform v10.4.4 / 10.4.5");

        return 0;
    }
}
