# Description
Now you are able to develop a normal minimal api application with the full power of .NET, with no other vendor-specific dependencies.

With a ASP .NET Core Minimal API, there is no longer **one** lambda per project.

The tooling will autpmatically create an API Gateway with resources to call each individual method in the controller.

It also includes an example on how to write and use a filter. More info:
https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-6.0

# How to create a ASP .NET Core Minimal API lambda
1. ```dotnet new serverless.AspNetCoreMinimalAPI --name MyFunctionName```
2. No .sln file will be created. If you wish to have one:
   1. ```cd .\MyFunctionName```
   2. ```dotnet new sln --name MyFunctionName```
   3. ```dotnet sln add .\src\MyFunctionName```
   
> ℹ️ Note: This template doesn't include a testing project

# Debug
The easiest way is to just debug from ```Visual Studio``` with the default profile, so you just debug your ASP .NET Core application.

I am trying to get the test lamba tool to work, but it's not yet ready. For more information: https://github.com/aws/aws-lambda-dotnet/issues/1095

# Deploy
1. Use an existing S3 bucket, or create a new one with: ```aws s3 mb s3://myfunctionname```
2. cd ```src\MyFunctionName```
3. ```dotnet lambda deploy-serverless```
4. Choose a stack name (```MyFunctionName```)
5. Enter the S3 bucket.
6. Wait.

An API Gateway is created and deployed automatically with the same name used for the function, wthi 2 stages: ```Stage``` and ```Prod```.

# Test deployed .NET6 lambda
1. Get the id of the automaticaly created API Gateway with: ```aws apigateway get-rest-apis --query 'items[?name==`MyFunctionName`].[id]' --output text```
2. Build the URL yourself: ```https://<api-gateway-id>.execute-api.<region>.amazonaws.com/<stageName>```
3. GET on that base URL should return the default welcome message, for example: ```Welcome to running ASP.NET Core Minimal API on AWS Lambda```
4. To call each controller method, use the controller name as resource, for example: ```GET https://xxxxxxx.execute-api.LOCATION.amazonaws.com/Stage/calculator/add/1/2```

See below the *original* ```Readme.md``` file created by the template:

---

# ASP.NET Core Minimal API Serverless Application

This project shows how to run an ASP.NET Core Web API project as an AWS Lambda exposed through Amazon API Gateway. The NuGet package [Amazon.Lambda.AspNetCoreServer](https://www.nuget.org/packages/Amazon.Lambda.AspNetCoreServer) contains a Lambda function that is used to translate requests from API Gateway into the ASP.NET Core framework and then the responses from ASP.NET Core back to API Gateway.


For more information about how the Amazon.Lambda.AspNetCoreServer package works and how to extend its behavior view its [README](https://github.com/aws/aws-lambda-dotnet/blob/master/Libraries/src/Amazon.Lambda.AspNetCoreServer/README.md) file in GitHub.

## Executable Assembly ##

.NET Lambda projects that use C# top level statements like this project must be deployed as an executable assembly instead of a class library. To indicate to Lambda that the .NET function is an executable assembly the 
Lambda function handler value is set to the .NET Assembly name. This is different then deploying as a class library where the function handler string includes the assembly, type and method name.

To deploy as an executable assembly the Lambda runtime client must be started to listen for incoming events to process. For an ASP.NET Core application the Lambda runtime client is started by included the
`Amazon.Lambda.AspNetCoreServer.Hosting` NuGet package and calling `AddAWSLambdaHosting(LambdaEventSource.HttpApi)` passing in the event source while configuring the services of the application. The
event source can be API Gateway REST API and HTTP API or Application Load Balancer.  

### Project Files ###

* serverless.template - an AWS CloudFormation Serverless Application Model template file for declaring your Serverless functions and other AWS resources
* aws-lambda-tools-defaults.json - default argument settings for use with Visual Studio and command line deployment tools for AWS
* Program.cs - entry point to the application that contains all of the top level statements initializing the ASP.NET Core application.
The call to `AddAWSLambdaHosting` configures the application to work in Lambda when it detects Lambda is the executing environment. 
* Controllers\CalculatorController - example Web API controller

You may also have a test project depending on the options selected.

## Here are some steps to follow from Visual Studio:

To deploy your Serverless application, right click the project in Solution Explorer and select *Publish to AWS Lambda*.

To view your deployed application open the Stack View window by double-clicking the stack name shown beneath the AWS CloudFormation node in the AWS Explorer tree. The Stack View also displays the root URL to your published application.

## Here are some steps to follow to get started from the command line:

Once you have edited your template and code you can deploy your application using the [Amazon.Lambda.Tools Global Tool](https://github.com/aws/aws-extensions-for-dotnet-cli#aws-lambda-amazonlambdatools) from the command line.

Install Amazon.Lambda.Tools Global Tools if not already installed.
```
    dotnet tool install -g Amazon.Lambda.Tools
```

If already installed check if new version is available.
```
    dotnet tool update -g Amazon.Lambda.Tools
```

Deploy application
```
    cd "LambdaWithMinimalApi/src/LambdaWithMinimalApi"
    dotnet lambda deploy-serverless
```
