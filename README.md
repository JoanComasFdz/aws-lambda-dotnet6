# aws-lambda-dotnet6
Contains examples, analysis and learnings of basic AWS Lambda functions implemented following the different AWS templates

Explore each solution and make sure to read the ```README.md``` file containing the analysis.

## Prerequisits
This repository is based on:
- Windows 10
- PowerShell 7
- Visual Studio Code
- .NET6 Runtime and SDK

### Create AWS user with programmatic access
Login or create a new Amazon AWS account.

1. Go to IAM
2. Go to Users
3. Go to Create Users
4. Choose a name, maybe something like ```devawscli``` or ```\<your-name\>vs```
5. Choose a password (May want to uncheck the change password on first login)
6. Click Next
7. Choose a name, like ```DevOps``` or ```AdminDev``` or ```Developers```
8. Assign AdminAccess policy
9. Clikc Create
10. Store the username, the password, the access key, the secret access key and the console URL. 
11. Download the csv.
12. Click Close

### Install the CLI tools
1. Open PowerShell as admin
2. Look for the latest version of Python 3: ```winget search “python 3”```
3. Install latest version of Python 3: ```winget install Python.Python.3.12```
4. Install AWS CLI: ```winget install Amazon.AWSCLI```
5. Close PowerShell

### Configure AWS CLI
1. Run: ```aws configure```
2. Enter the access key you stored
3. Enter the secret access key you stored
4. Choose a default session (maybe look for the cheaper one, like ```us-west-1```)
5. Do not establish an output format, just click Enter

### Test AWS CLI
1. Run: ```aws s3 ls```

### Set up the AWS Kit for Visual Studio
1. Install the [AWSToolkitforVisualStudio2022](https://marketplace.visualstudio.com/items?itemName=AmazonWebServices.AWSToolkitforVisualStudio2022)
2. Open Visual Studio
3. The AWS Explorer pane should be now visible on the left. Otherwise use the View menu to open it.

### Install the dotnet extensions
1. Install the AWS CLI extensions for dotnet: ```dotnet tool install -g Amazon.Lambda.Tools```
2. Install the AWS Templates: ```dotnet new -i Amazon.Lambda.Templates```
3. List lambda templates: ```dotnet new list Lambda```

---

## Debug a .NET6 Lambda
The easiest way is to just debug from ```Visual Studio```, which will launch a browser window with a page to test the lambda. 

For automation reasons, use the tool used by VS to run a lambda. Look into the ```launchProperties.json``` and you will find all you need:

```
"commandLineArgs": "--port 5050",
"workingDirectory": ".\\bin\\$(Configuration)\\net6.0",
"executablePath": "%USERPROFILE%\\.dotnet\\tools\\dotnet-lambda-test-tool-6.0.exe"
```

Make sure to ```dotnet build``` or unit test ```dotnet test``` your lambda beforehand, so that it creates the necessary files in ```/bin```.

1. Open PowerShell
2. Go to the root folder of your lambda
2. For autpmation, avoid launching a window with ```--no-launch-window```
3. Run the tool as:


```
PS \MyFunctionName\src\MyFunctionName\bin\Release\net6.0> C:\Users\MY-USER-NAME\.dotnet\tools\dotnet-lambda-test-tool-6.0.exe --port 5050 --no-launch-window
```

More info on the tool: https://github.com/aws/aws-lambda-dotnet/blob/master/Tools/LambdaTestTool/README.md

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