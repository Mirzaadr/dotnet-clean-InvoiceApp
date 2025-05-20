namespace InvoiceApp.Domain.Commons.Models;

public class PagedList<T>
{
    public PagedList(List<T> items, int page, int pageSize, int totalCount)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    public List<T> Items { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNext => Page * PageSize < TotalCount;
    public bool HasPrev => Page > 1;

    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> query, int page, int pageSize)
    {
        var totalCount = await Task.FromResult(query.Count());
        var items = await Task.FromResult(query.Skip((page - 1) * pageSize).Take(pageSize).ToList());

        return new(items, page, pageSize, totalCount);
    }
}