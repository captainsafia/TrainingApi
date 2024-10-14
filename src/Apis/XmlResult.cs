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
        builder.Metadata.Add(new XmlResponseTypeMetadata(typeof(T)));
    }
}

public class XmlResponseTypeMetadata : IProducesResponseTypeMetadata
{
    public XmlResponseTypeMetadata(Type? type)
    {
        Type = type;
    }

    public Type? Type { get; set; }

    public int StatusCode { get; set; } = StatusCodes.Status200OK;

    public IEnumerable<string> ContentTypes { get; set; } = ["application/xml"];
}

public static class XmlResultExtensions
{
    public static XmlResult<T> Xml<T>(this IResultExtensions _, T result) => new(result);
}