using System.CommandLine;
using System.CommandLine.Invocation;
using System.Reflection;

namespace AlchiwebApp.Cli;

public class CustomVersionAction : SynchronousCommandLineAction
{

    public override int Invoke(ParseResult parseResult)
    {
        var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;

        Console.WriteLine($"Version: {assemblyVersion} / Tested with BitPlatform v10.4.4 / 10.4.5");

        return 0;
    }
}
