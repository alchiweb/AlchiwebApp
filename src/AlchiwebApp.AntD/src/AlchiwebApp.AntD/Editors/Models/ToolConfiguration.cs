using AlchiwebApp.AntD.Editors.Models.Options;
using System.Text.Json.Serialization;

namespace AlchiwebApp.AntD.Editors.Models
{
    public class ToolConfiguration<TOptions> where TOptions : ToolOptions
    {
        public LoadActions LoadActions { get; set; } = new LoadActions();
        [JsonPropertyName("options")]
        public TOptions? Options { get; set; }
        public ToolConfiguration(string? overrideOptionsKey = null, string? loadProviderClassFunctionDefault = null, NamingSchemeEnum optionsNamingScheme = NamingSchemeEnum.CamelCase)
        {
            LoadActions.OverrideOptionsKey = overrideOptionsKey ??
                typeof(TOptions) switch
                    {
                        Type codeType when codeType == typeof(CodeToolOptions) => "code",
                        Type simpleImageType when simpleImageType == typeof(SimpleImageOptions) => "image",
                        Type nestedListType when nestedListType == typeof(NestedListOptions) => "list",
                        _ => null
                    };
            LoadActions.OptionsNamingScheme = optionsNamingScheme;
            LoadActions.LoadProviderClassFunctionDefault = loadProviderClassFunctionDefault;
        }
    }
}
