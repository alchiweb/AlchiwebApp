using System.Text.Json.Serialization;

namespace AlchiwebApp.PagingFiltering.Filtering;

public record FieldFilter<TFieldEnum, TField>
    where TFieldEnum : struct, Enum
    where TField : notnull
{
    [JsonPropertyName("c")]
    public TFieldEnum Comparator { get; set; }
    [JsonPropertyName("s")]
    public TField SearchValue { get; set; }
    [JsonPropertyName("o")]
    public bool IsOrCondition { get; set; }

    [JsonConstructor]
    public FieldFilter(TFieldEnum comparator, TField searchValue, bool isOrCondition)
    {
        Comparator = comparator;
        SearchValue = searchValue;
        IsOrCondition = isOrCondition;
    }
}
