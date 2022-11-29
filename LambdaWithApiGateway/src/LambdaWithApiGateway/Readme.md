# Description

This is the easiest lambda you can create that can be triggered from an API Gateway.

### How to create the simples lambda
1. ```dotnet new lambda.EmptyFunction --name MyFunctionName```
2. No .sln file will be created. If you wish to have one:
   1. ```cd .\MyFunctionName```
   2. ```dotnet new sln --name MyFunctionName```
   3. ```dotnet sln add .\src\MyFunctionName .\test\MyFunctionName.Tests```
3. ```cd .\src\MyFunctionName```
4. ```dotnet add package Amazon.Lambda.APIGatewayEvents```
5. ```cd ..\..\test\LambdaWithApiGateway.Tests\```
6. ```dotnet add package Amazon.Lambda.APIGatewayEvents```

No update the code of the ```FunctionHandler``` method:
```
public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
{
    return new APIGatewayProxyResponse()
    {
        Body = input.Body.ToUpper(),
        StatusCode = 200
    };
}
```

And the tests:
```
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
```

# Test deployed .NET6 lambda
The payload is different due to the new request type, it should look like:
```
{"body": "hello world"}
```

For example:
```
dotnet lambda invoke-function dotnet6LambdaWithApiGateway --payload "{""body"": ""hello world""}"
```


---

Omitted *original* ```Readme.md``` file created by the template, since it is the same as in BasicLambda.