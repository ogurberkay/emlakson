namespace Core.Results.Filter;

public class PaginationFilter
{
    const int maxPageSize = 50;
    public int PageNumber { get; set; } = 1;
    private int _pageSize = 9;
    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}

public enum OrderByEnum
{
    Latest,
    PriceDesc,
    PriceAsc
}