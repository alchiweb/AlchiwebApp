using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace Alchiweb-App1.Core.AlchiwebApp;

public static class LocalizationHelper
{
    public static readonly CultureInfo[] SUPPORTED_CULTURES =
        [
            new CultureInfo(DEFAULT_CULTURE),
            new CultureInfo("fr"),
        ];
    public const string DEFAULT_CULTURE = "en";

}
