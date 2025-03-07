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

During OpenAPI generation, the framework searches for XML doc comments in the current assembly and project references.
If the XML doc comments are found, they are included in the OpenAPI document.

Note that the C# build process does not capture XML doc comments placed on lamda expresions, so to use XML doc comments to add metadata to a minimal API endpoint, you must define the endpoint handler as a method, put the XML doc comments on the method, and then reference that method from the `MapXXX` method. For example, to use XML doc comments to add metadata to a minimal API endpoint originally defined as a lambda expression:

```csharp
app.MapGet("/hello", (string name) =>$"Hello, {name}!");
```

You would change the endpoint to be defined as a method:

```csharp
app.MapGet("/hello", Hello);

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

The example above illustrates the <summary>, <remarks>, and <param> XML doc comments.
For more information about XML doc comments, see the [C# documentation](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags).

## Upgrade to OpenAPI.NET v2.0.0-preview7

The OpenAPI.NET library used in ASP.NET Core OpenAPI document generation has been upgraded to v2.0.0-preview7.
This version includes a number of bug fixes and improvements and also introduces some breaking changes.
The breaking changes should only impact users that use document, operation, or schema transformers.
Most of the changes fall into one of the following categories:
- Properties changed from concrete types to interfaces.
- The introduction of reference types for many OpenAPI types, for example in addition to `OpenApiParameter` there is now an `OpenApiParameterReference` type.

## Treating empty string in form post as null for nullable value types

Empty string values in a form post are now converted to null rather than causing a parse failure.

Community contribution! Thanks @nvmkpk!

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

