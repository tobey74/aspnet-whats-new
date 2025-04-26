using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace SseInMvc.Controllers;

[ApiController]
public class SseController : ControllerBase
{
    [HttpGet("/string-item")]
    public ServerSentEventsResult<string> Get(CancellationToken cancellationToken)
    {
        async IAsyncEnumerable<string> GetHeartRate(
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var heartRate = Random.Shared.Next(60, 100);
                yield return $"Hear Rate: {heartRate} bpm";
                await Task.Delay(2000, cancellationToken);
            }
        }

        return TypedResults.ServerSentEvents(GetHeartRate(cancellationToken), eventType: "heartRate");
    }

    [HttpGet("/json-item")]
    public ServerSentEventsResult<HeartRateEvent> GetJson(CancellationToken cancellationToken)
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
    }

    [HttpGet("/sse-item")]
    public ServerSentEventsResult<int> GetSse(CancellationToken cancellationToken)
    {
        async IAsyncEnumerable<SseItem<int>> GetHeartRate(
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var heartRate = Random.Shared.Next(60, 100);
                yield return new SseItem<int>(heartRate, eventType: "heartRate"){
                    ReconnectionInterval = TimeSpan.FromMinutes(1)
                };
                await Task.Delay(2000, cancellationToken);
            }
        }

        return TypedResults.ServerSentEvents(GetHeartRate(cancellationToken));
    }
}

public record HeartRateEvent(DateTime Timestamp, int HeartRate)
{
    public static HeartRateEvent Create(int heartRate) => new(DateTime.UtcNow, heartRate);
}