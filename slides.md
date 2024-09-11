---
layout: cover
---

# minimal apis, _enhanced_
## a presentation by @captainsafia

<!-- Good morning, everyone! Thanks for coming to my session. I'm Safia, a software engineer on the ASP.NET Core team working mostly on minimal APIs. And today, I want to talk to you about minimal APIs and how you can go above and beyond with them. -->

---

# Goals

<v-clicks>

## - .NET 6 => .NET 8 (and beyond)
## - Understanding how the framework is evolving
## - Learn how framework features can be assembled into more complex APIs
## - Controller-based APIs vs minimal APIs

</v-clicks>

<!-- 
Before we get started with the core content of this presentation, I wanted to share some goals that I have for what you'll get out of this talk.

By this end of the talk, I hope that you'll have a better of understanding of how minimal APIs compare to the classic controller-based APIs.

If you're someone who started using minimal APIs when they were originally released in .NET 6, this talk should give you a sense of the changes that have happened since then, especially if you're jumping from .NET 6 LTS to .NET 8 LTS.

-->

---
layout: image
image: ./images/app-screenshot.png
backgroundSize: contain
---

<!-- The app that we will be using for this presentation is a little TrainingApi that provides endpoints for managing trainers and clients at a gym. Now, this is meant to be a follow along session where we will browse through the code in this application and discuss some of the key features. -->

---
layout: image
image: ./images/gh-repo-qrcode.png
backgroundSize: contain
---

<!-- If you'd like, you can scan this QR code to navigate to a code browser with the app that we are exploring so you can see what lines of code I am referring to as we go through each line of code. -->

---

# Linear configuration comes to constructing authorization policies (.NET 7)

````md magic-move
```csharp
builder.Services.AddAuthorization(options =>
    options.AddPolicy("trainer_access", policy =>
        policy.RequireRole("trainer").RequireClaim("permission", "admin"))
);
```
```csharp
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("trainer_access", policy => 
        policy.RequireRole("trainer").RequireClaim("permission", "admin"));
```
````

---

# Built-in OpenAPI support (.NET 9)

```csharp
// OpenAPI dependencies
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Set up OpenAPI-related endpoints
    app.MapOpenApi();
    app.MapScalarApiReference();
}
```

---

# `TypedResults` for strongly-typed responses (.NET 7)

````md magic-move
```csharp
app.MapGet("/", () => Results.Redirect("/scalar/v1"))
    .ExcludeFromDescription();
```
```csharp
app.MapGet("/", () => TypedResults.Redirect("/scalar/v1"))
    .ExcludeFromDescription();
```
```csharp
// Arrange
var clientToCreate = new Client(4, "Gretchen", "Beslier",
    "gbeslier0@nba.com", 311, 65,
    DateTime.Parse("7/22/1984", CultureInfo.InvariantCulture));
var mockContext = CreateMockDbContext();
var service = new TrainingService(mockContext.Object);

// Act
var result = service.CreateClient(clientToCreate);

// Assert
var typedResult = Assert.IsType<Created<Client>>(result.Result);
Assert.Equal(StatusCodes.Status201Created, typedResult.StatusCode);
Assert.Equal(clientToCreate, typedResult.Value);    
```
````

---

# Support for route groups (.NET 7)

```csharp
var clients = app.MapGroup("/clients/{id}");

clients.MapGet("", (int id, TrainingService service) => service.GetClientById(id));
clients.MapPut("", (int id, Client updatedClient, TrainingService service)
            => service.UpdateClientById(id, updatedClient));

var trainers = app.MapGroup("/trainers")
    .RequireAuthorization("trainer_access");

trainers.MapGet("/", (TrainingService service, PagingData pagingData)
    => service.GetTrainers(pagingData.ItemCount, pagingData.CurrentPage));
trainers.MapPut("/{id}", (int id, Trainer updatedTrainer, TrainingService service)
    => service.UpdateTrainerById(id, updatedTrainer));
trainers.MapDelete("/{id}", (int id, TrainingService service) => service.DeleteTrainerById(id));
trainers.MapPost("/", (TrainingService service, Trainer trainer) => service.CreateTrainer(trainer));
```

---

# Support for endpoint filters (.NET 7)

```csharp
clients.MapPut("", (int id, Client updatedClient, TrainingService service)
    => service.UpdateClientById(id, updatedClient))
    .AddEndpointFilter(async (context, next) =>
    {
        var client = context.GetArgument<Client>(2);
        Dictionary<string, string[]> errors = new();
        if (client.FirstName.Any(char.IsDigit))
        {
            errors.Add("firstName", ["First names cannot contain any numeric characters."]);
        }
        if (client.LastName.Any(char.IsDigit))
        {
            errors.Add("lastName", ["Last names cannot contain any numeric characters."]);
        }
        if (errors.Count() > 0)
        {
            return TypedResults.ValidationProblem(errors);
        }
        return await next(context);
    });
```

---

# Support for extending Results types (.NET 7)

````md magic-move
```csharp
public async Task<Results<XmlResult<List<Trainer>>, NotFound>> GetTrainers()
{
    var trainers = await _db.Trainers.ToListAsync();
    return Results.Extensions.Xml(trainers);
}
```
````
---

# Support for `IEndpointMetadataProvider` implementations (.NET 7)

```csharp
public class XmlResult<T> : IResult, IEndpointMetadataProvider
{
    public static void PopulateMetadata(MethodInfo method, EndpointBuilder builder)
    {
        builder.Metadata.Add(new XmlResponseTypeMetadata(typeof(T)));
    }
}
```

---

# Support for custom parameter binding (.NET 7)

````md magic-move
```csharp
trainers.MapGet("/", (TrainingService service, PagingData pagingData)
    => service.GetTrainers(pagingData.ItemCount, pagingData.CurrentPage));
```
```csharp
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
```
````

---

# **And more...**

<v-clicks>

## - Generate JWT tokens locally for testing with `dotnet user-jwts` (.NET 7)
## - Support for native AoT (.NET 8)
## - Support for form binding from complex types (.NET 8)

</v-clicks>

---
layout: cover
text-class: center
---

# Thanks!
# Questions?

---