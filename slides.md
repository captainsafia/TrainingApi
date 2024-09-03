---
layout: cover
background: https://cover.sli.dev
---

# Minimal APIs
# _Enhanced_
## Safia Abdalla || Principal Software Engineer || Microsoft

<!-- Good morning, everyone! Thanks for coming to my session. I'm Safia, a software engineer on the ASP.NET Core team working mostly on minimal APIs. And today, I want to talk to you about minimal APIs and how you can go above and beyond with them. -->

---

# Goals

<v-clicks>

## - Controller-based APIs versus minimal APIs
## - .NET 6 => .NET 8 (and beyond)
## - Understanding how the framework is evolving

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

<!-- If you'd like, you can scan this QR code to navigate to a code browser with the app that we are exploring so you can see what lines of code I am referring to as we go through each line of code. -->

---

# Linear configuration comes to constructing authorization policies

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
var clientToCreate = new Client(4, "Gretchen", "Beslier", "gbeslier0@nba.com", 311, 65, DateTime.Parse("7/22/1984", CultureInfo.InvariantCulture));
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

var trainers = app.MapGroup("/trainers")
    .RequireAuthorization("trainer_access");
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

```

---

# Support for custom parameter binding (.NET 7)

---

# Support for native AoT (.NET 8)

---
layout: cover
---

# Thanks!
# Questions?

---