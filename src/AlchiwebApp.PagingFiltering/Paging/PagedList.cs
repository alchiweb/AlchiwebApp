using System.Text.Json.Serialization;

namespace AlchiwebApp.PagingFiltering.Paging;

public class PagedList<T> : IPagedList
{
    [JsonConstructor]
    public PagedList(List<T> items, Pagination pagination)
    {
        Items = items;
        Pagination = pagination;
        //TotalCount = count;
        //PageSize = pageSize;
        //CurrentPage = pageNumber;
        //TotalPages = (uint)Math.Ceiling(count / (double)pageSize);
    }

    public List<T> Items { get; }

    public Pagination Pagination { get; }
    
    //public uint CurrentPage { get; private set; }

    //public uint TotalPages { get; private set; }

    //public uint PageSize { get; private set; }

    //public uint TotalCount { get; private set; }

    //public bool HasPreviousPage => CurrentPage > 1;

    //public bool HasNextPage => CurrentPage < TotalPages;
}
//public class PagedList<T> : PagedList<T, Guid>
//{
//    public PagedList() : base()
//    {
//    }
//    public PagedList(IEnumerable<T> items, Pagination pagination) : base(items, pagination)
//    {
//    }
//}
