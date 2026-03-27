namespace AlchiwebApp.AntD.Editors.Models
{
    /// <summary>
    /// Load actions configuration.
    /// </summary>
    public class LoadActions
    {
        /// <summary>
        /// Naming scheme for options keys.
        /// </summary>
        public NamingSchemeEnum OptionsNamingScheme { get; set; } = NamingSchemeEnum.CamelCase;
        /// <summary>
        /// Load provider class function default.
        /// </summary>
        public string? LoadProviderClassFunctionDefault { get; set; }
        /// <summary>
        /// Override options key.
        /// </summary>
        public string? OverrideOptionsKey { get; set; }
    }
}
