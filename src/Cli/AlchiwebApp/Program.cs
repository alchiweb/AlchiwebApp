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
        rootCommand.Options.Where(opt => opt as VersionOption != null)?.FirstOrDefault()?.Action = new CustomVersionAction();

        // Mod command
        Command modCommand = new("mod-bp", "Modify generated BitPlatform app.") {
            targetDirectoryOption,
            sourceDirectoryOption,
            showExpectedReplacementsOption
        };
        modCommand.SetAction(async (parseResult) =>
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
        rootCommand.Subcommands.Add(modCommand);

        // Add AlchiwebApp command
        Command upgradeCommand = new("upgrade", "Upgrade a modified BitPlatform app to AlchiwebApp.") {
            targetDirectoryOption,
            showExpectedReplacementsOption
        };
        upgradeCommand.SetAction(async (parseResult) =>
        {
            if (parseResult.GetRequiredValue(targetDirectoryOption) is DirectoryInfo targetParsedDirectory
                && parseResult.GetValue(showExpectedReplacementsOption) is bool showExpectedReplacements
            )
            {
                var bitPlatformAppModUpgrade = new BitPlatformAppModUpgrade(targetParsedDirectory.FullName, showExpectedReplacements, new FileSearchService());
                await bitPlatformAppModUpgrade.AddAlchiwebApp();
            }
        });
        rootCommand.Subcommands.Add(upgradeCommand);

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
