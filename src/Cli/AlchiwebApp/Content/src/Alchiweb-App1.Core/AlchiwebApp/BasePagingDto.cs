using System.Diagnostics.CodeAnalysis;
using AlchiwebApp.PagingFiltering.Filtering;
using AlchiwebApp.PagingFiltering.Paging;

namespace Alchiweb-App1.Core.AlchiwebApp;


public record BasePagingDto<TSortingEnum, T> : BasePagingDto<TSortingEnum>
    where TSortingEnum : struct, Enum
    where T : class, new()
{
    public BasePagingDto(TSortingEnum? defaultSorting = null, int? defaultPageSize = null, int? defaultPage = null) : base(defaultSorting, defaultPageSize, defaultPage)
    {
    }

    public static T Parse(string s, IFormatProvider? provider)
    {
        TryParse(s, provider, out var result);
        return result ?? new T();
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out T result)
    {
        result = null;
        if (string.IsNullOrEmpty(s))
        {
            return false;
        }
        T? deserializerResult = null;
        try
        {
            deserializerResult = JsonSerializer.Deserialize(s, typeof(T), AppJsonContext.Default) as T;
        }
        catch (Exception)
        { }
        if (deserializerResult == null)
        {
            return false;
        }
        result = deserializerResult;
        return true;
    }
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, typeof(T), AppJsonContext.Default);
    }

}


public record BasePagingDto<TSortingEnum> : BasePagingDto where TSortingEnum : struct, Enum
{
    [JsonPropertyName("o")]
    public Dictionary<TSortingEnum, SortingInfos> SortBy { get; set; } = [];

    public BasePagingDto(TSortingEnum? defaultSorting = null, int? defaultPageSize = null, int? defaultPage = null) : base(defaultPageSize, defaultPage)
    {
        if (defaultSorting != null)
            SortBy.Add(defaultSorting.Value, new SortingInfos(0, false));
    }
}

public record BasePagingDto : IBasePaging
{
    [JsonPropertyName("p")]
    public int? Page { get; set; }
    [JsonPropertyName("s")]
    public int? PageSize { get; set; }
    public BasePagingDto(int? defaultPageSize = null, int? defaultPage = null)
    {
        if (defaultPageSize != null)
            PageSize = defaultPageSize;
        if (defaultPage != null)
            PageSize = defaultPage;
    }

}



