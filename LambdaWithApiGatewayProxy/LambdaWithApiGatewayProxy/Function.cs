using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace LambdaWithApiGatewayProxy;

public class Function
{
    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
    {
        var patient = JsonConvert.DeserializeObject<Patient>(input.Body);

        context.Logger.LogInformation($"Received patient: {patient}");

        return new APIGatewayProxyResponse
        {
            Body = $"Patient created: {patient}",
            StatusCode = 200,
        };
    }
}


public record Patient (int Id, string Name);