using Amazon.Lambda.APIGatewayEvents;
using Xunit;
using Amazon.Lambda.TestUtilities;

namespace LambdaWithApiGateway.Tests;

public class FunctionTest
{
    [Fact]
    public void TestToUpperFunction()
    {
        var request = new APIGatewayProxyRequest { Body = "hello world"};
        var context = new TestLambdaContext();
        var function = new Function(); 
        var response = function.FunctionHandler(request, context);

        Assert.Equal(200, response.StatusCode);
        Assert.Equal("HELLO WORLD", response.Body);
    }
}
