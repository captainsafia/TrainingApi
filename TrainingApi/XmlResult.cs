using System.Reflection;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http.Metadata;

public class XmlResult<T> : IResult, IEndpointMetadataProvider
{
    private static readonly XmlSerializer _xmlSerializer = new(typeof(T));

    private readonly T _result;

    public XmlResult(T result)
    {
        _result = result;
    }

    public Task ExecuteAsync(HttpContext httpContext)
    {
        // Pool this for better efficiency
        using var ms = new MemoryStream();

        _xmlSerializer.Serialize(ms, _result);

        httpContext.Response.ContentType = "application/xml";

        ms.Position = 0;
        return ms.CopyToAsync(httpContext.Response.Body);
    }

    public static void PopulateMetadata(MethodInfo method, EndpointBuilder builder)
    {
        builder.Metadata.Add(new ProducesResponseTypeMetadata
        {
            Type = typeof(T),
            StatusCode = StatusCodes.Status200OK,
            ContentTypes = new[] { "application/xml" },
        });
    }

    private class ProducesResponseTypeMetadata : IProducesResponseTypeMetadata
    {
        public Type? Type { get; set; }

        public int StatusCode { get; set; }

        public IEnumerable<string> ContentTypes { get; set; } = Array.Empty<string>();
    }
}

public static class XmlResultExtensions
{
    public static XmlResult<T> Xml<T>(this IResultExtensions _, T result) => new XmlResult<T>(result);
}
