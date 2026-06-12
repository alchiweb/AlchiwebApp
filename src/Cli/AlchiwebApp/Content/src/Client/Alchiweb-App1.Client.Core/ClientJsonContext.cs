namespace Alchiweb-App1.Client.Core;

/// <summary>
/// JsonSerializerContext for the client layer, statically compiled for better performance (AOT compatible).
/// </summary>
[JsonSourceGenerationOptions(

    //    WriteIndented = false,
    //GenerationMode = JsonSourceGenerationMode.Metadata,
    //    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    //    ReferenceHandler = JsonKnownReferenceHandler.Preserve // NEW in .NET 10
        PropertyNameCaseInsensitive = true
    //DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
)]

//[JsonSerializable(typeof(MenuDataItem))]
//[JsonSerializable(typeof(NavLinkMatch))]
[JsonSerializable(typeof(RenderFragment))]
//[JsonSerializable(typeof(TestMenuDataItem[]))]

public partial class ClientJsonContext : JsonSerializerContext { }

