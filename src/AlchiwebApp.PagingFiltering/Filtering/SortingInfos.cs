using System.Text.Json.Serialization;

namespace AlchiwebApp.PagingFiltering.Filtering;

public record SortingInfos
{
    [JsonPropertyName("p")]
    public int Priority { get; }
    [JsonPropertyName("r")]
    public bool IsReversed { get; }

    [JsonConstructor]
    public SortingInfos(int priority = 0, bool isReversed = false)
    {
        Priority = priority;
        IsReversed = isReversed;
    }
}
