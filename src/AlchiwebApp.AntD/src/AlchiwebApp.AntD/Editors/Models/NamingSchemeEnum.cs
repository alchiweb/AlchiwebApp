using System.Text.Json.Serialization;

namespace AlchiwebApp.AntD.Editors.Models
{
    /// <summary>
    /// Naming scheme enum.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<NamingSchemeEnum>))]
    public enum NamingSchemeEnum
    {
        /// <summary>
        /// CamelCase (e.g. myVariableName)
        /// </summary>
        CamelCase = 0,
        /// <summary>
        /// PascalCase (e.g. MyVariableName)
        /// </summary>
        PascalCase,
        /// <summary>
        /// SnakeCase (e.g. my_variable_name)
        /// </summary>
        SnakeCase,
        /// <summary>
        /// KebabCase (e.g. my-variable-name)
        /// </summary>
        KebabCase
    }
}
