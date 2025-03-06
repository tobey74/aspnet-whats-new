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

ASP.NET Core OpenAPI document generation wlll now include metadata from XML doc comments on on method, class, and member definitions in the OpenAPI document. During OpenAPI generation, the framework searches for XML doc comments in the current assembly and project references. If the XML doc comments are found, they are included in the OpenAPI document.

## Upgrade to OpenAPI.NET v2.0.0-preview7

The OpenAPI.NET library used in ASP.NET Core OpenAPI document generation has been upgraded to v2.0.0-preview7.
This version includes a number of bug fixes and improvements and also introduces some breaking changes. The breaking changes should only impact users that use document, operation, or schema transformers. For the most part, the

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

