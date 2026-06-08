using System.CommandLine;
using System.CommandLine.Parsing;
using System.Reflection;
using AlchiwebApp.Cli.Core;
using AlchiwebApp.Cli.Core.Services;

namespace AlchiwebApp.Cli;

internal class Program
{
    private static async Task<int> Main(string[] args)
    {
        var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
        Option<DirectoryInfo> targetDirectoryOption = new("--target", "-t")
        {
            Description = "The folder of the newly created BitPlatform application (to modify)",
            Required = true,
            //DefaultValueFactory = parseResult => new DirectoryInfo("."),
            HelpName = "BitPlatform folder",
            Arity = ArgumentArity.ExactlyOne,
            Validators =
            {
                optionResult => DirectoryValidation(optionResult)
            }
        };
        Option<DirectoryInfo> sourceDirectoryOption = new("--source", "-s")
        {
            Description = "The folder of the source BitPlatform application (to read parameters)",
            Required = true,
            //DefaultValueFactory = parseResult => new DirectoryInfo(Path.Combine("..", "..")),
            HelpName = "BitPlatform folder",
            Arity = ArgumentArity.ExactlyOne,
            Validators =
            {
                optionResult => DirectoryValidation(optionResult)
            }
        };
        Option<bool> showExpectedReplacementsOption = new("--expected-replacements", "-er")
        {
            Description = "Show the expected replacements for a standard project (PostgreSQL, API Standalone, Pipeline Azure, Admin, Sample)",
            Required = false,
            DefaultValueFactory = parseResult => false,
            HelpName = "",
            Arity = ArgumentArity.Zero,
        };
        Console.WriteLine($"AddToBitPlatform v{assemblyVersion}");
        RootCommand rootCommand = new($"AlchiwebApp / Add To BitPlatform v{assemblyVersion} (tested with BitPlatform v10.4.4 / v14.4.5)");
        rootCommand.Options.Add(targetDirectoryOption);
        rootCommand.Options.Add(sourceDirectoryOption);
        rootCommand.Options.Add(showExpectedReplacementsOption);
        rootCommand.SetAction(async (parseResult) =>
        {
            if (parseResult.GetRequiredValue(targetDirectoryOption) is DirectoryInfo targetParsedDirectory
                && parseResult.GetRequiredValue(sourceDirectoryOption) is DirectoryInfo sourceParsedDirectory
                && parseResult.GetValue(showExpectedReplacementsOption) is bool showExpectedReplacements
            )
            {
                var bitPlatformAppMod = new BitPlatformAppMod(targetParsedDirectory.FullName, sourceParsedDirectory.FullName, showExpectedReplacements, new FileSearchService());
                await bitPlatformAppMod.ModifyBitPlatformProject();
            }
        });

        for (int i = 0; i < rootCommand.Options.Count; i++)
        {
            // RootCommand has a default VersionOption; update its Action.
            if (rootCommand.Options[i] is VersionOption defaultVersionOption)
            {
                defaultVersionOption.Action = new CustomVersionAction();
                break;
            }
        }

        ParseResult parseResult = rootCommand.Parse(args);
        parseResult.Invoke();

        return parseResult.Errors.Count == 0 ? 0 : 1;
    }

    private static bool DirectoryValidation(OptionResult optionResult)
    {
        DirectoryInfo? optionValue = null;
        try
        {
            optionValue = optionResult.GetValueOrDefault<DirectoryInfo>();
            if (Directory.Exists(optionValue.FullName) == false)
            {
                optionResult.AddError("Folder does not exist.");
                return false;
            }
        }
        catch (Exception)
        {
            optionResult.AddError("Invalid folder path.");
            return false;
        }
        return true;
    }
}
