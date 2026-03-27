using AlchiwebApp.AntD.Editors.Abstractions.Models;
using AlchiwebApp.AntD.Editors.Abstractions.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AlchiwebApp.AntD.Editors.Abstractions.Utils;

/// <summary>
/// JsonSerializerContext for EditorJs, statically compiled for better performance (AOT compatible).
/// </summary>
[JsonSourceGenerationOptions(

    //    WriteIndented = false,
    //GenerationMode = JsonSourceGenerationMode.Metadata,
    //    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    //    ReferenceHandler = JsonKnownReferenceHandler.Preserve // NEW in .NET 10
    PropertyNameCaseInsensitive = true,
    Converters = [typeof(JsonStringEnumConverter<BlockTypeEnum>)],
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
)]

[JsonSerializable(typeof(BlockTypeEnum))]

[JsonSerializable(typeof(EditorJsBlock))]
[JsonSerializable(typeof(EditorJsBlock<AttachesData>))]
[JsonSerializable(typeof(EditorJsBlock<ChecklistData>))]
[JsonSerializable(typeof(EditorJsBlock<CodeData>))]
[JsonSerializable(typeof(EditorJsBlock<DelimiterData>))]
[JsonSerializable(typeof(EditorJsBlock<EmbedData>))]
[JsonSerializable(typeof(EditorJsBlock<HeaderData>))]
[JsonSerializable(typeof(EditorJsBlock<ImageData>))]
[JsonSerializable(typeof(EditorJsBlock<LinkToolData>))]
[JsonSerializable(typeof(EditorJsBlock<ListData>))]
[JsonSerializable(typeof(EditorJsBlock<ParagraphData>))]
[JsonSerializable(typeof(EditorJsBlock<PersonalityData>))]
[JsonSerializable(typeof(EditorJsBlock<QuoteData>))]
[JsonSerializable(typeof(EditorJsBlock<RawData>))]
[JsonSerializable(typeof(EditorJsBlock<TableData>))]
[JsonSerializable(typeof(EditorJsBlock<TextData>))]
[JsonSerializable(typeof(EditorJsBlock<WarningData>))]

public partial class EditorJsJsonContext : JsonSerializerContext { }

