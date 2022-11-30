# Description
This lambda can be called from an API Gateway.

Only 1 funtion per project is allowed.

This lambda in particular does not support GET of the input parameter. For a GET method, just delete de input parameter entirely.

### How to create a .NET 6 lambda that can be called from an API Gateway
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

## Deploy
1. Navigate to the folder containing the Function.cs file
2. Run: ```dotnet lambda deploy-function```
3. Choose a lambda name: Uniquely and descriptevly identifies your lambda
4. Choose a role name or create a new one (again, use a descritpeve name, an starter can be ```MyFunctionName```)
5. Choose IAM policy (wait for the list to appear): USe ```AWSLambdaExecute```. You can also start with ```AWSLambda_FullAccess``` to avoid hassle now and choose a more limited one later

## Test a deployed .NET6 lambda
1. Run: ```dotnet lambda invoke-function MyFunctionName --payload "{"body": "hello world"}"```
2. The output should be similar to:
```
PS BasicLambda2\src\BasicLambda2> dotnet lambda invoke-function MyFunctionName --payload "{"body": "hello world"}"
Amazon Lambda Tools for .NET Core applications (5.6.2)
Project Home: https://github.com/aws/aws-extensions-for-dotnet-cli, https://github.com/aws/aws-lambda-dotnet

Payload:
"HELLO WORLD"

Log Tail:
START RequestId: 0e3b2d83-1e91-42ec-b164-ed6190908b21 Version: $LATEST
END RequestId: 0e3b2d83-1e91-42ec-b164-ed6190908b21
REPORT RequestId: 0e3b2d83-1e91-42ec-b164-ed6190908b21  Duration: 226.91 ms     Billed Duration: 227 ms Memory Size: 256 MB     Max Memory Used: 70 MB
```

## Access via API Gateway
It is not recommended to expose lambdas directly to the Internet, they should be behind an API Gateway.

1. Once deployed, go to the aws console
2. Go to ```Lambda```
3. Click on your deployed lambda.
4. Wait and click to ```Add trigger```
5. Choose ```API Gateway``` (existing or new REST and Open security)
6. Click ```Add```
7. Wait and click on the API Gateway name.
8. On ```ANY```, select ```Test```
9. Select verb ```POST```
10. Enter a valid JSON or and empty string (```""```).
11. Click on ```Test```
12. Study the right panel from the top

When you create a resource, make sure to check ```Use Lambda Proxy integration```.

When it works as desired:
1. Click on ```Actions```
2. Click on ```Deploy API```
3. Select ```default``` stage
4. Enter some description
5. Click on ```Deploy```
6. Copy the url.
7. Make sure to add the resource name of the endpoint at the end of the lambda.

Use your favourite tool to test it, For example in REST Client:
```
POST https://xxxxxxxxx.execute-api.us-west-1.amazonaws.com/default/myFunctionName
content-type: application/application/json

{
 "body": "hello workd"
}
```

## Return codes and outputs

When called from an API Gatway test page or from the internet, this is how it will behave:

- GET: 500 Internal server error + error message in response
- POST: 200 OK + response
- PUT: 200 OK + response
- DELETE: 200 OK + response
- PATCH: 200 OK + response
- OPTIONS: 200 OK + response
- HEAD: 200 OK + no response


---

Omitted *original* ```Readme.md``` file created by the template, since it is the same as in BasicLambda.