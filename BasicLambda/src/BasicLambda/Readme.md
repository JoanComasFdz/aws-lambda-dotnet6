# Description

This is the easiest lambda you can create.

It receives a string and returns a string.

Keep in mind that it will *not* work with an API Gateway or SQS because the ``` FunctionHandler``` method needs some particular types.

Only 1 function per project is allowed.

### How to create the simples lambda
1. Run: ```dotnet new lambda.EmptyFunction --name MyFunctionName```
3. ```cd .\MyFunctionName```
3. No ```.sln``` file will be created. If you wish to have one:
   1. ```dotnet new sln --name MyFunctionName```
   2. ```dotnet sln add .\src\MyFunctionName .\test\MyFunctionName.Tests```

## Deploy a .NET6 lambda
1. Navigate to the folder containing the Function.cs file
2. Run: ```dotnet lambda deploy-function```
3. Choose a lambda name: Uniquely and descriptevly identifies your lambda
4. Choose a role name or create a new one (again, use a descritpeve name, an starter can be ```MyFunctionName```)
5. Choose IAM policy (wait for the list to appear): USe ```AWSLambdaExecute```. You can also start with ```AWSLambda_FullAccess``` to avoid hassle now and choose a more limited one later

## Test a deployed .NET6 lambda
1. Run: ```dotnet lambda invoke-function MyFunctionName --payload "hello world"```
2. The output should be similar to:
```
PS BasicLambda2\src\BasicLambda2> dotnet lambda invoke-function MyFunctionName --payload "hello world"
Amazon Lambda Tools for .NET Core applications (5.6.2)
Project Home: https://github.com/aws/aws-extensions-for-dotnet-cli, https://github.com/aws/aws-lambda-dotnet

Payload:
"HELLO WORLD"

Log Tail:
START RequestId: 0e3b2d83-1e91-42ec-b164-ed6190908b21 Version: $LATEST
END RequestId: 0e3b2d83-1e91-42ec-b164-ed6190908b21
REPORT RequestId: 0e3b2d83-1e91-42ec-b164-ed6190908b21  Duration: 226.91 ms     Billed Duration: 227 ms Memory Size: 256 MB     Max Memory Used: 70 MB
```

See below the *original* ```Readme.md``` file created by the template:

---

# AWS Lambda Empty Function Project

This starter project consists of:
* Function.cs - class file containing a class with a single function handler method
* aws-lambda-tools-defaults.json - default argument settings for use with Visual Studio and command line deployment tools for AWS

You may also have a test project depending on the options selected.

The generated function handler is a simple method accepting a string argument that returns the uppercase equivalent of the input string. Replace the body of this method, and parameters, to suit your needs. 

## Here are some steps to follow from Visual Studio:

To deploy your function to AWS Lambda, right click the project in Solution Explorer and select *Publish to AWS Lambda*.

To view your deployed function open its Function View window by double-clicking the function name shown beneath the AWS Lambda node in the AWS Explorer tree.

To perform testing against your deployed function use the Test Invoke tab in the opened Function View window.

To configure event sources for your deployed function, for example to have your function invoked when an object is created in an Amazon S3 bucket, use the Event Sources tab in the opened Function View window.

To update the runtime configuration of your deployed function use the Configuration tab in the opened Function View window.

To view execution logs of invocations of your function use the Logs tab in the opened Function View window.

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

Execute unit tests
```
    cd "BasicLambda/test/BasicLambda.Tests"
    dotnet test
```

Deploy function to AWS Lambda
```
    cd "BasicLambda/src/BasicLambda"
    dotnet lambda deploy-function
```
