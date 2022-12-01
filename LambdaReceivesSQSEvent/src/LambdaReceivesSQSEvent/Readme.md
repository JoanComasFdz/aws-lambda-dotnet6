# Description
Nothing very intersting here, just receive an event and process its records.

When debugging, use the Example Request ```SQS```, to change the event content just modify the body property.

# How to create a lambda that receives an SQS Event
1. ```dotnet new lambda.SQS --name MyFunctionName```
2. No .sln file will be created. If you wish to have one:
   1. ```cd .\MyFunctionName```
   2. ```dotnet new sln --name MyFunctionName```
   3. ```dotnet sln add .\src\MyFunctionName```

# Debug
Just debug from ```Visual Studio``` with the default profile.

# Deploy
1. Navigate to the folder containing the Function.cs file
2. Run: ```dotnet lambda deploy-function```
3. Choose a lambda name: Uniquely and descriptevly identifies your lambda
4. Choose a role name or create a new one (again, use a descritpeve name, an starter can be ```MyFunctionName```)
5. Choose IAM policy (wait for the list to appear): USe ```AWSLambdaSQSQueueExecutionRole```

# Test a deployed .NET6 lambda
1. Run: ```dotnet lambda invoke-function MyFunctionName --payload "{""Records"": [{ ""body"": ""Hello from SQS!"" }]}"```
2. The output should be similar to:
```
PS > dotnet lambda invoke-function dotnet6LambdaReceivesSQSEvent --payload "{""Records"": [{ ""body"": ""Hello from SQS!"" }]}"
Amazon Lambda Tools for .NET Core applications (5.6.2)
Project Home: https://github.com/aws/aws-extensions-for-dotnet-cli, https://github.com/aws/aws-lambda-dotnet

Payload:


Log Tail:
START RequestId: 2448ca0c-b816-4d67-9ae5-dd4454380af6 Version: $LATEST
2022-12-01T10:19:30.772Z        2448ca0c-b816-4d67-9ae5-dd4454380af6    info    Processed record Hello from SNS!
END RequestId: 2448ca0c-b816-4d67-9ae5-dd4454380af6
REPORT RequestId: 2448ca0c-b816-4d67-9ae5-dd4454380af6  Duration: 94.42 ms      Billed Duration: 95 ms  Memory Size: 256 MB     Max Memory Used: 66 MB
```

# Access via SQS

## By hand
### Create a queue
1. Go to the AWS Console
2. Go to ```Simple Queue Service```
3. Click ```Create new```
4. Leave the default values
5. Choose a descriptive name, like ```MyFunctionName```
6. Click ```Create```
7. Wait until its created, should take less than 1min.

### Make the queue trigger the lambda
1. Go to ```Lambda```
2. Click on your lambda's name
3. Click on ```Add trigger```
4. Select ```SQS```
5. Select the one you just created
6. Levae default values
7. Click ```Add```

### Test it
1. Click on the queue name
2. Click on ```Send and receive messages```
3. Write something in the body
4. Click ```Send```
5. Go back to your lambda
6. Click on ```Monitoring```
7. Click on the latest CloudWatch log group
8. CloudWatch will open in a new tab
8. Scroll down
9. You should see an ```info``` message with what you wrote in the body.

## CLI
### Create a queue
1. Run: ```aws sqs create-queue --queue-name MyQueue```
2. Store the URL returned

### Make the queue trigger the lambda
1. Get the queue ARN: ```aws sqs get-queue-attributes --queue-url https://sqs.LOCATION.amazonaws.com/111111111/MyQueue --attribute-name QueueArn```
2. Run: ```aws lambda create-event-source-mapping --function-name MyFunctionName --event-source-arn arn:aws:sqs:LOCATION:111111111:MyQueue```

### Test it
1. The log group name is: ```/aws/lambda/MyFunctionName```
2. Open a new PowerShell window
3. Run: ```aws logs tail /aws/lambda/MyFunctionName --follow```
4. Go back to the previous windows
5. Run: ```aws sqs send-message --queue-url https://sqs.LOCATION.amazonaws.com/111111111/MyQueue --message-body "Sending a message from CLI."```
6. Go back to the other window
7. After few seconds, the logs will appear, like:

```
PS> aws logs tail /aws/lambda/MyFunctionName --follow
2022-12-01T09:23:18.631000+00:00 2022/12/01/[$LATEST]650d7977477d4d34b6e56c20e08796ba START RequestId: 85855d17-69b3-5604-b7b2-01151d00e719 Version: $LATEST
2022-12-01T09:23:18.981000+00:00 2022/12/01/[$LATEST]650d7977477d4d34b6e56c20e08796ba 2022-12-01T09:23:18.924Z  85855d17-69b3-5604-b7b2-01151d00e719    info    Processed message Sending a message from CLI.
2022-12-01T09:23:19.064000+00:00 2022/12/01/[$LATEST]650d7977477d4d34b6e56c20e08796ba END RequestId: 85855d17-69b3-5604-b7b2-01151d00e719
2022-12-01T09:23:19.064000+00:00 2022/12/01/[$LATEST]650d7977477d4d34b6e56c20e08796ba REPORT RequestId: 85855d17-69b3-5604-b7b2-01151d00e719    Duration: 433.61 ms     Billed Duration: 434 ms     Memory Size: 256 MB     Max Memory Used: 61 MB  Init Duration: 203.56 ms
```


See below the *original* ```Readme.md``` file created by the template:

---
# AWS Lambda Simple SQS Function Project

This starter project consists of:
* Function.cs - class file containing a class with a single function handler method
* aws-lambda-tools-defaults.json - default argument settings for use with Visual Studio and command line deployment tools for AWS

You may also have a test project depending on the options selected.

The generated function handler responds to events on an Amazon SQS queue.

After deploying your function you must configure an Amazon SQS queue as an event source to trigger your Lambda function.

## Here are some steps to follow from Visual Studio:

To deploy your function to AWS Lambda, right click the project in Solution Explorer and select *Publish to AWS Lambda*.

To view your deployed function open its Function View window by double-clicking the function name shown beneath the AWS Lambda node in the AWS Explorer tree.

To perform testing against your deployed function use the Test Invoke tab in the opened Function View window.

To configure event sources for your deployed function use the Event Sources tab in the opened Function View window.

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
    cd "LambdaReceivesSQSEvent/test/LambdaReceivesSQSEvent.Tests"
    dotnet test
```

Deploy function to AWS Lambda
```
    cd "LambdaReceivesSQSEvent/src/LambdaReceivesSQSEvent"
    dotnet lambda deploy-function
```
