using System.Reflection;

public class PaginationData
{
    public int CurrentPage { get; init; } = 1;
    public int ItemCount { get; init; } = 5;

    public static ValueTask<PaginationData?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        const string itemCountKey = "itemCount";
        const string currentPageKey = "page";

        int.TryParse(context.Request.Query[itemCountKey], out var itemCount);
        int.TryParse(context.Request.Query[currentPageKey], out var page);
        page = page == 0 ? 1 : page;

        var result = new PaginationData
        {
            ItemCount = itemCount,
            CurrentPage = page
        };

        return ValueTask.FromResult<PaginationData?>(result);
    }
}
