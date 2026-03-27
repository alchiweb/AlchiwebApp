using System.Text.Json.Serialization;

namespace AlchiwebApp.PagingFiltering.Filtering;

public record BaseFilter<TSortingEnum> : BaseFilter<TSortingEnum, Dictionary<TSortingEnum, SortingInfos>>
    where TSortingEnum : struct, Enum
{

    [JsonConstructor]
    public BaseFilter(Dictionary<TSortingEnum, SortingInfos>? sortBy) : base(sortBy)
    {
    }
}
public record BaseFilter<TSortingEnum, TDictionary>
    where TSortingEnum : struct, Enum
    where TDictionary : Dictionary<TSortingEnum, SortingInfos>, new()
{
    [JsonPropertyName("o")]
    public TDictionary SortBy { get; }


    [JsonConstructor]
    public BaseFilter(TDictionary? sortBy)
    {
        SortBy = sortBy ?? [];
    }
}
