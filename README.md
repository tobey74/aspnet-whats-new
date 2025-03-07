# aspnet-whats-new

What's new in ASP.NET for .NET 10 preview 2.

## Overview

Here's a list of the new features and improvements in ASP.NET for .NET 10 preview 2.

<!-- https://github.com/dotnet/aspnetcore/pulls?q=is%3Apr+milestone%3A10.0-preview2+is%3Aclosed+label%3Aarea-minimal%2Carea-mvc%2Carea-auth%2Carea-identity -->

- [Populate XML doc comments into OpenAPI document #60326](https://github.com/dotnet/aspnetcore/pull/60326)
- [Upgrade to OpenAPI.NET v2.0.0-preview7 #60269](https://github.com/dotnet/aspnetcore/pull/60269)
- [Treating empty string in form post as null for nullable value types #52499](https://github.com/dotnet/aspnetcore/pull/52499)
- [Add AuthN/AuthZ metrics #59557](https://github.com/dotnet/aspnetcore/pull/59557)

## Populate XML doc comments into OpenAPI document

ASP.NET Core OpenAPI document generation wlll now include metadata from XML doc comments on on method, class, and member definitions in the OpenAPI document. You must enable XML doc comments in your project file to use this feature. You can do this by adding the following property to your project file:

```xml
  <PropertyGroup>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
  </PropertyGroup>
```

At build-time, the OpenAPI package will leverage a source generator to discover XML comments in the current application assembly and any project references and emit source code to insert them into the document via an OpenAPI document transformer.

Note that the C# build process does not capture XML doc comments placed on lamda expresions, so to use XML doc comments to add metadata to a minimal API endpoint, you must define the endpoint handler as a method, put the XML doc comments on the method, and then reference that method from the `MapXXX` method. For example, to use XML doc comments to add metadata to a minimal API endpoint originally defined as a lambda expression:

```csharp
app.MapGet("/hello", (string name) =>$"Hello, {name}!");
```

You would change the endpoint to be defined as a method:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

if (app.Environment.IsDeelopment())
{
  app.MapOpenApi();
}

app.MapGet("/hello", Hello);

app.Run();

// Define the endpoint handler as a method.

/// <summary>
/// Sends a greeting.
/// </summary>
/// <remarks>
/// Greeting a person by their name.
/// </remarks>
/// <param name="name">The name of the person to greet.</param>
/// <returns>A greeting.</returns>
public static string Hello(string name)
{
    return $"Hello, {name}!";
}
```

The example above illustrates the `<summary>`, `<remarks>`, and `<param>` XML doc comments.
For more information about XML doc comments, including all the supported tags, see the [C# documentation](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags).

Since the core functionality is provided via a source generator, it can be disabled by adding the following MSBuild to your project file.

```
<ItemGroup>
  <PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="10.0.0-preview.2.*" GeneratePathProperty="true" />
</ItemGroup>

<Target Name="DisableCompileTimeOpenApiXmlGenerator" BeforeTargets="CoreCompile">
  <ItemGroup>
    <Analyzer Remove="$(PkgMicrosoft_AspNetCore_OpenApi)/analyzers/dotnet/cs/Microsoft.AspNetCore.OpenApi.SourceGenerators.dll" />
  </ItemGroup>
</Target>
```

The source generator process XML files included in the `AdditionalFiles` property. To add (or remove), sources modify the property as follows:

```
<Target Name="AddXmlSources" BeforeTargets="CoreCompile">
  <ItemGroup>
    <AdditionalFiles Include="$(PkgSome_Package/lib/net10.0/Some.Package.xml" />
  </ItemGroup>
</Target>
```

## Upgrade to OpenAPI.NET v2.0.0-preview7

The OpenAPI.NET library used in ASP.NET Core OpenAPI document generation has been upgraded to v2.0.0-preview7. This version includes a number of bug fixes and improvements and also introduces some breaking changes. The breaking changes should only impact users that use document, operation, or schema transformers. Breaking changes in this iteration include the following:

- Entities within the OpenAPI document, like operations and parameters, are typed as interfaces. Concrete implementationse exist for the inlined and referenced variants of an entity. For example, an `IOpenApiSchema` can be an inlined `OpenApiSchema` or an `OpenApiSchemaReference` that points to a schema referenced from elsewhere in the document.
- The `Nullable` property has been removed from the `OpenApiSchema` type. To determine if a type is nullable, evaluate if the `OpenApiSchema.Type` property sets `JsonSchemaType.Null`.

## Treating empty string in form post as null for nullable value types

When using the `[FromForm]` attribute with a complex object in minimal APIs, empty string values in a form post are now converted to null rather than causing a parse failure. This behavior matches the processing logic for form posts not associated with complex object's in minimal APIs.

```csharp
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapPost("/todo", ([FromForm] Todo todo) => TypedResults.Ok(todo));

app.Run();

public class Todo
{
  public int Id { get; set; }
  public DateOnly? DueDate { get; set; } // Empty strings map to `null`
  public string Title { get; set; }
  public bool IsCompleted { get; set; }
}
```

Thanks to @nvmkpk for contributing this change!

## Add AuthN/AuthZ metrics

This PR adds metrics for certain authentication and authorization events in ASP.NET Core. With this change, you can now obtain metrics for the following events:
- Authentication:
  - Authenticated request duration
  - Challenge count
  - Forbid count
  - Sign in count
  - Sign out count
- Authorization:
  - Count of requests requiring authorization
