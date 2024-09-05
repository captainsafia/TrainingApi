using System.Reflection;

public class PagingData
{
    public int ItemCount { get; init; } = 10;
    public int CurrentPage { get; init; } = 1;

    public static ValueTask<PagingData?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        const string ItemCountKey = "itemCount";
        const string currentPageKey = "page";

        int.TryParse(context.Request.Query[ItemCountKey], out var itemCount);
        int.TryParse(context.Request.Query[currentPageKey], out var page);
        page = page == 0 ? 1 : page;

        var result = new PagingData
        {
            ItemCount = itemCount,
            CurrentPage = page
        };

        return ValueTask.FromResult<PagingData?>(result);
    }
}