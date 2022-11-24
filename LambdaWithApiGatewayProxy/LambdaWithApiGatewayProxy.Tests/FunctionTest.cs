using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using Xunit;

namespace LambdaWithApiGatewayProxy.Tests;

public class FunctionTest
{
    [Fact]
    public void TestToUpperFunction()
    {

        // Invoke the lambda function and confirm the string was upper cased.
        var function = new Function();
        var context = new TestLambdaContext();
        // { "id": "123", "name": "Joan" }
        var request = new APIGatewayProxyRequest
        {
            Body = "{ \"Id\": \"321\", \"Name\": \"Johnny\" }"
        };
        var response = function.FunctionHandler(request, context);

        Assert.Equal(200, response.StatusCode);
    }
}
