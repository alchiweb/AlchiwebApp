using AlchiwebApp.AntD.Editors.Abstractions.Models;
using AlchiwebApp.AntD.Editors.Abstractions.Models.Data;
using AlchiwebApp.AntD.Editors.Models;
using AlchiwebApp.AntD.Editors.Models.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AlchiwebApp.AntD.Utils;

/// <summary>
/// JsonSerializerContext for EditorJsConfiguration, statically compiled for better performance (AOT compatible).
/// </summary>
[JsonSourceGenerationOptions(

    //    WriteIndented = false,
    //GenerationMode = JsonSourceGenerationMode.Metadata,
    //    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    //    ReferenceHandler = JsonKnownReferenceHandler.Preserve // NEW in .NET 10
    PropertyNameCaseInsensitive = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
)]

[JsonSerializable(typeof(NamingSchemeEnum))]
[JsonSerializable(typeof(ListCounterTypeEnum))]
[JsonSerializable(typeof(ListStyleEnum))]

[JsonSerializable(typeof(EditorJsConfiguration))]
[JsonSerializable(typeof(ToolsConfigurations))]

public partial class EditorJsConfigurationJsonContext : JsonSerializerContext { }

