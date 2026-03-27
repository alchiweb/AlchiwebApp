namespace AlchiwebApp.PagingFiltering.Paging;

public interface IBasePaging
{
    int? Page { get; set; }
    int? PageSize { get; set; }
}
