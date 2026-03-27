using AlchiwebApp.PagingFiltering.Filtering;

namespace AlchiwebApp.PagingFiltering.Paging;


public record BasePaging<TSortingEnum> : BasePaging<TSortingEnum, Dictionary<TSortingEnum, SortingInfos>>
    where TSortingEnum : struct, Enum
{
    public BasePaging(TSortingEnum? defaultSorting = null, int? defaultPageSize = null, int? defaultPage = null) : base(defaultSorting, defaultPageSize, defaultPage)
    { }
}
    public record BasePaging<TSortingEnum,TDictionary> : BaseFilter<TSortingEnum, TDictionary>, IBasePaging
    where TSortingEnum : struct, Enum
    where TDictionary : Dictionary<TSortingEnum, SortingInfos>, new()
{
    [JsonPropertyName("p")]
    public int? Page { get; set; }
    [JsonPropertyName("s")]
    public int? PageSize { get; set; }
    public BasePaging(TSortingEnum? defaultSorting = null, int? defaultPageSize = null, int? defaultPage = null) : base([])
    {
        if (defaultPageSize != null)
            PageSize = defaultPageSize;
        if (defaultPage != null)
            PageSize = defaultPage;
        if (defaultSorting != null)
            SortBy.Add(defaultSorting.Value, new SortingInfos(0, false));
    }
}

public record BasePaging : IBasePaging
{
    public int? Page { get; set; }
    public int? PageSize { get; set; }
    public BasePaging(int? defaultPageSize = null, int? defaultPage = null)
    {
        if (defaultPageSize != null)
            PageSize = defaultPageSize;
        if (defaultPage != null)
            PageSize = defaultPage;
    }

}

