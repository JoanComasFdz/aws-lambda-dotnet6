using System.Text.Json;
using Amazon.SimpleNotificationService;

namespace LambdaMinimalApiPublishesOnSNS;

public interface ISnsPublisher
{
    Task PublishAsync<T>(T message);
}

public class SnsPublisher : ISnsPublisher
{
    private readonly AmazonSimpleNotificationServiceClient _client;
    private readonly string _topicArn;

    public SnsPublisher(AmazonSimpleNotificationServiceClient client, string topicArn)
    {
        _client = client;
        _topicArn = topicArn;
    }

    public async Task PublishAsync<T>(T message)
    {
        await _client.PublishAsync(_topicArn, JsonSerializer.Serialize(message));
    }
}