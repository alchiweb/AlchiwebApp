using AlchiwebApp.AntD.Editors.Abstractions.Models;
using AlchiwebApp.AntD.Editors.Abstractions.Models.Data;
using AlchiwebApp.AntD.Editors.Abstractions.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlchiwebApp.AntD.Editors.Abstractions.JsonConverters;
/// <summary>
/// JsonConverter for EditorJsBlock that handles polymorphic deserialization
/// </summary>
public class EditorJsDataJsonConverter : JsonConverter<EditorJsBlock>
{
    /// <summary>
    /// Reads and converts the JSON to type EditorJsBlock.
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public override EditorJsBlock Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        JsonElement root = JsonElement.ParseValue(ref reader);
        BlockTypeEnum blockTypeEnum = BlockTypeEnum.Paragraph;
        string? id = null;
        try
        {
            blockTypeEnum = (JsonSerializer.Deserialize(root.GetProperty("type"), typeof(BlockTypeEnum), EditorJsJsonContext.Default) as BlockTypeEnum?) ?? BlockTypeEnum.Paragraph;
        } catch { }
        try
        {
            id = root.GetProperty("id").GetString();
        }
        catch { }
        if (string.IsNullOrWhiteSpace(id))
            id = Guid.NewGuid().ToString("N");

        EditorJsBlock? editorJsBlock = null;

        if (root.TryGetProperty("data", out JsonElement dataElement))
        {
            editorJsBlock = blockTypeEnum switch
            {
                BlockTypeEnum.Attaches => CreateBlock<AttachesData>(id, blockTypeEnum, dataElement),
                BlockTypeEnum.Checklist => CreateBlock<ChecklistData>(id, blockTypeEnum, dataElement),
                BlockTypeEnum.Code => CreateBlock<CodeData>(id, blockTypeEnum, dataElement),
                BlockTypeEnum.Delimiter => CreateBlock<DelimiterData>(id, blockTypeEnum, dataElement),
                BlockTypeEnum.Embed => CreateBlock<EmbedData>(id, blockTypeEnum, dataElement),
                BlockTypeEnum.Header => CreateBlock<HeaderData>(id, blockTypeEnum, dataElement),
                BlockTypeEnum.Image => CreateBlock<ImageData>(id, blockTypeEnum, dataElement),
                BlockTypeEnum.LinkTool => CreateBlock<LinkToolData>(id, blockTypeEnum, dataElement),
                BlockTypeEnum.List => CreateBlock<ListData>(id, blockTypeEnum, dataElement),
                BlockTypeEnum.Paragraph => CreateBlock<ParagraphData>(id, blockTypeEnum, dataElement),
                BlockTypeEnum.Personality => CreateBlock<PersonalityData>(id, blockTypeEnum, dataElement),
                BlockTypeEnum.Quote => CreateBlock<QuoteData>(id, blockTypeEnum, dataElement),
                BlockTypeEnum.Raw => CreateBlock<RawData>(id, blockTypeEnum, dataElement),
                BlockTypeEnum.Table => CreateBlock<TableData>(id, blockTypeEnum, dataElement),
                BlockTypeEnum.Warning => CreateBlock<WarningData>(id, blockTypeEnum, dataElement),
                BlockTypeEnum.Text => CreateBlock<TextData>(id, blockTypeEnum, dataElement),
                _ => null
            };
        }
        return editorJsBlock ?? CreateBlock<ParagraphData>(id, blockTypeEnum, dataElement);
    }

    /// <summary>
    /// Writes a specified value as JSON.
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public override void Write(
        Utf8JsonWriter writer,
        EditorJsBlock value,
        JsonSerializerOptions options)

    {
        writer.WriteStartObject();
        writer.WritePropertyName("id");
        writer.WriteStringValue(value.Id);
        writer.WritePropertyName("type");
        JsonSerializer.Serialize(writer, value.Type, typeof(BlockTypeEnum), EditorJsJsonContext.Default);

        switch (value)
        {
            case EditorJsBlock<CodeData> block:
                SerializeDataBlock(writer, block);
                break;
            case EditorJsBlock<ParagraphData> block:
                SerializeDataBlock(writer, block);
                break;
            case EditorJsBlock<HeaderData> block:
                SerializeDataBlock(writer, block);
                break;
            case EditorJsBlock<ListData> block:
                SerializeDataBlock(writer, block);
                break;
            case EditorJsBlock<QuoteData> block:
                SerializeDataBlock(writer, block);
                break;
            case EditorJsBlock<ChecklistData> block:
                SerializeDataBlock(writer, block);
                break;
            case EditorJsBlock<TableData> block:
                SerializeDataBlock(writer, block);
                break;
            case EditorJsBlock<ImageData> block:
                SerializeDataBlock(writer, block);
                break;
            case EditorJsBlock<DelimiterData> block:
                SerializeDataBlock(writer, block);
                break;
            case EditorJsBlock<WarningData> block:
                SerializeDataBlock(writer, block);
                break;
            case EditorJsBlock<EmbedData> block:
                SerializeDataBlock(writer, block);
                break;
                //case EditorJsBlock<TextData> block:
                //    SerializeDataBlock(writer, block);
                //    break;
        }
        writer.WriteEndObject();
    }

    private static EditorJsBlock CreateBlock<TBlockData>(string id, BlockTypeEnum type, JsonElement dataElement) where TBlockData : class, IBlockData
    {
        EditorJsBlock editorJsBlock;
        editorJsBlock = new EditorJsBlock<TBlockData>()
        {
            Id = id,
            Type = type,
            Data = JsonSerializer.Deserialize(dataElement, typeof(TBlockData), EditorJsJsonContext.Default) as TBlockData
        };
        return editorJsBlock;
    }

    private static void SerializeDataBlock<TBlockData>(Utf8JsonWriter writer, EditorJsBlock<TBlockData> value) where TBlockData : class, IBlockData
    {
        writer.WritePropertyName("data");
        JsonSerializer.Serialize(writer, value.Data, typeof(TBlockData), EditorJsJsonContext.Default);
    }

}
/*

public class EditorJsDataJsonConverter<TBlockData> : JsonConverter<EditorJsBlock<TBlockData>> where TBlockData : class, IBlockData
{
    public override EditorJsBlock<TBlockData> Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        JsonElement root = JsonElement.ParseValue(ref reader);
        BlockTypeEnum blockTypeEnum = BlockTypeEnum.Paragraph;
        string? id = null;
        try
        {
            blockTypeEnum = JsonSerializer.Deserialize<BlockTypeEnum>(root.GetProperty("type"), options);
        }
        catch { }
        try
        {
            id = root.GetProperty("id").GetString();
        }
        catch { }
        if (string.IsNullOrWhiteSpace(id))
            id = Guid.NewGuid().ToString("N");

        EditorJsBlock<TBlockData> editorJsBlock = new()
        {
            Id = id,
            Type = blockTypeEnum,
        };

        if (root.TryGetProperty("data", out JsonElement dataElement))
        {
            editorJsBlock.Data = JsonSerializer.Deserialize<TBlockData>(dataElement);
            //    editorJsBlock.Type switch
            //{
            //    BlockTypeEnum.Attaches => JsonSerializer.Deserialize<TBlockData>(dataElement),
            //    BlockTypeEnum.Checklist => JsonSerializer.Deserialize<ChecklistData>(dataElement),
            //    BlockTypeEnum.Code => JsonSerializer.Deserialize<CodeData>(dataElement),
            //    BlockTypeEnum.Delimiter => JsonSerializer.Deserialize<DelimiterData>(dataElement),
            //    BlockTypeEnum.Embed => JsonSerializer.Deserialize<EmbedData>(dataElement),
            //    BlockTypeEnum.Header => JsonSerializer.Deserialize<HeaderData>(dataElement),
            //    BlockTypeEnum.Image => JsonSerializer.Deserialize<ImageData>(dataElement),
            //    BlockTypeEnum.LinkTool => JsonSerializer.Deserialize<LinkToolData>(dataElement),
            //    BlockTypeEnum.List => JsonSerializer.Deserialize<ListData>(dataElement),
            //    BlockTypeEnum.Paragraph => JsonSerializer.Deserialize<ParagraphData>(dataElement),
            //    BlockTypeEnum.Personality => JsonSerializer.Deserialize<PersonalityData>(dataElement),
            //    BlockTypeEnum.Quote => JsonSerializer.Deserialize<QuoteData>(dataElement),
            //    BlockTypeEnum.Raw => JsonSerializer.Deserialize<RawData>(dataElement),
            //    BlockTypeEnum.Table => JsonSerializer.Deserialize<TableData>(dataElement),
            //    BlockTypeEnum.Warning => JsonSerializer.Deserialize<WarningData>(dataElement),
            //    _ => null
            //};
        }
        return editorJsBlock;
    }


    public override void Write(
        Utf8JsonWriter writer,
        EditorJsBlock<TBlockData> value,
        JsonSerializerOptions options)

    {
        writer.WriteStartObject();
        writer.WritePropertyName("id");
        writer.WriteStringValue(value.Id);
        writer.WritePropertyName("type");
        JsonSerializer.Serialize(writer, value.Type, options);
        if (value.Data is not null)
        {
            writer.WritePropertyName("data");
            JsonSerializer.Serialize(writer, value.Data, options);
        }
        writer.WriteEndObject();
    }
}
*/