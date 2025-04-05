# aspnet-whats-new

What's new in ASP.NET for .NET 10 Preview 3

- Validation support in Minimal APIs
- Support for SSE (Server-Sent Events) in Minimal APIs
- Setting the Description in a ProducesResponseTypeAttribute works correctly for Minimal API
  - https://github.com/dotnet/aspnetcore/pull/60539
- Add Microsoft.AspNetCore.OpenApi to the webapiaot template
  - https://github.com/dotnet/aspnetcore/pull/60337

<!-- https://github.com/dotnet/AspNetCore.Docs/issues/34948 -->

## Validation support in Minimal APIs

<!-- https://github.com/captainsafia/minapi-validation-support -->

Support for validation in minimal APIs is now available. This feature allows you to request validation of data
sent to your API endpoints. When validation is enabled, the ASP.NET Core runtime will perform any validations
defined on query, header, and route parameters, as well as on the request body.
Validations can be defined using attributes in the `System.ComponentModel.DataAnnotations` namespace.
Developers can customize the behavior of the validation system by:

- creating custom [ValidationAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.validationattribute?view=net-9.0) implementations
- implement the [IValidatableObject](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.ivalidatableobject?view=net-9.0) interface for complex validation logic

When validation fails, the runtime will return a 400 Bad Request response with
details of the validation errors.

To enable built-in validation support for minimal APIs, call the `AddValidation` extension method to register
the required services into the service container for your application.

```csharp
builder.Services.AddValidation();
```

The implementation automatically discovers types that are defined in minimal API handlers or as base types of types defined in minimal API handlers. Validation is then performed on these types by an endpoint filter that is added for each endpoint.

Validation can be disabled for specific endpoints by using the `DisableValidation` extension method.

```csharp
app.MapPost("/products",
    ([EvenNumber(ErrorMessage = "Product ID must be even")] int productId, [Required] string name)
        => TypedResults.Ok(productId))
    .DisableValidation();
```

<!-- Validation in Minimal APIs is designed to be AOT-friendly. The validation logic is generated at build time, which means that it can be used in AOT scenarios without any additional configuration. This makes it easy to use validation in your minimal APIs without worrying about runtime performance. -->

## Support for Server-Sent Events (SSE)

<!-- Sample repo and docs are available at https://github.com/captainsafia/minapi-sse. -->

ASP.NET Core now supports returning a `ServerSentEvents` result using the `TypedResults.ServerSentEvents` API.
This feature is supported in both Minimal APIs and controller-based apps.

Server-Sent Events (SSE) is a server push technology that allows a server to send a stream of event messages to a
client over a single HTTP connection. In .NET the event messages are represented as [SseItem<T>](https://learn.microsoft.com/en-us/dotnet/api/system.net.serversentevents.sseitem-1) objects, which may contain an event type, an ID, and a data payload of type `T`.

The `TypedResults` class has a new static method called `ServerSentEvents` that can be used to return a `ServerSentEvents` result. The first parameter to this method is an `IAsyncEnumerable<SseItem<T>>` that represents the stream of event messages to be sent to the client.

The following example illustrates how to the  `TypedResults.ServerSentEvents` API to return a stream of heart rate events as JSON objects to the client:

```csharp
app.MapGet("/json-item", (CancellationToken cancellationToken) =>
{
    async IAsyncEnumerable<HeartRateEvent> GetHeartRate(
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var heartRate = Random.Shared.Next(60, 100);
            yield return HeartRateEvent.Create(heartRate);
            await Task.Delay(2000, cancellationToken);
        }
    }

    return TypedResults.ServerSentEvents(GetHeartRate(cancellationToken), eventType: "heartRate");
});
```

When OpenAPI document generation is enabled, endpoints that return a `ServerSentEvents` result will be documented with the `text/event-stream` content type.

## Setting Response Descriptions in Minimal API

The `Description` parameter of the `ProducesAttribute`, `ProducesResponseTypeAttribute`, and `ProducesDefaultResponseAttribute`
is now supported in Minimal APIs. This allows you to set a description for the response type in the OpenAPI document.

Here's an example:

```csharp
app.MapGet("/weatherforecast",
    [ProducesResponseType<IEnumerable<WeatherForecast>>(StatusCodes.Status200OK, Description = "The service is healthy.")]
    () =>
```

And the generated OpenAPI will be

```json
        "responses": {
          "200": {
            "description": "The weather forecast for the next 5 days.",
            "content": {
```

This was a community contribution by @sander1095. Thanks for this contribution!

## Add Microsoft.AspNetCore.OpenApi to the webapiaot template

Support for OpenAPI document generation with the Microsoft.AspNetCore.OpenApi packagte is now included by default in the webapiaot project template. This support can be disabled if desired by using the `--no-openapi` flag when creating a new project.

This was a community contribution by @sander1095. Thanks for this contribution!
