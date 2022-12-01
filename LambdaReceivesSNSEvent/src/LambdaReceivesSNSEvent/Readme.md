# Description
Nothing very intersting here, just receive an event and process its records.

This example will subscribe directly to a message published in an SNS topic, but in reality most applications will **not** do that.

The standard procedure is:
1. Application publishes event to an SNS Topic
2. SNS Topic routes the event to the necessary SQS Queues, based on some rules
3. SQS Queues deliver the event to the subscribed aplications

> ℹ️ Note: Even if conceptually queues are to be pulled from, lambdas will just get the message pushed.

More info: https://medium.com/awesome-cloud/aws-difference-between-sqs-and-sns-61a397bf76c5

When debugging, use the Example Request ```SNS```, to change the event content just modify the body property.

# How to create a lambda that receives an SQS Event
1. ```dotnet new lambda.SNS --name MyFunctionName```
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
5. Choose IAM policy (wait for the list to appear): USe ```AWSLambda_FullAccess```
6. **Add CloudWatch rights**: ```aws iam attach-role-policy --policy-arn arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole --role-name MyFunctionN ame```

# Test a deployed .NET6 lambda
1. Run: ```dotnet lambda invoke-function MyFunctionName --payload "{ ""Records"": [ { ""Sns"": { ""Message"": ""Hello from SNS!"" }}]}"```
2. The output should be similar to:
```
PS > dotnet lambda invoke-function MyFunctionName --payload "{ ""Records"": [ { ""Sns"": { ""Message"": ""Hello from SNS!"" }}]}"
Amazon Lambda Tools for .NET Core applications (5.6.2)
Project Home: https://github.com/aws/aws-extensions-for-dotnet-cli, https://github.com/aws/aws-lambda-dotnet

Payload:


Log Tail:
START RequestId: fb6378ea-3801-4ee4-bd5e-59183ba16ed6 Version: $LATEST
2022-12-01T08:40:03.793Z        fb6378ea-3801-4ee4-bd5e-59183ba16ed6    info    Processed message Hello from SQS!
END RequestId: fb6378ea-3801-4ee4-bd5e-59183ba16ed6
REPORT RequestId: fb6378ea-3801-4ee4-bd5e-59183ba16ed6  Duration: 74.86 ms      Billed Duration: 75 ms  Memory Size: 256 MB     Max Memory Used: 70 MB
```

# Access via SNS
As of now, Lambda only supports ```Standard SNS``` topics.

To create one, follow the steps in [LambdaReceivesSQSEvent Readme.md](../../../LambdaReceivesSQSEvent/src/LambdaReceivesSQSEvent/Readme.md), selecting ```FIFO``` type.

## By hand
### Create a topic
1. Go to the AWS Console
2. Go to ```Simple Notification Service```
3. Go to ```Topics```
3. Click ```Create new```
4. Select ```Standard```
5. Choose a descriptive name, like ```MyFunctionName```
6. In Access policies, select ```All``` in Who can publish and Who can subscribe.
6. Click ```Create```

### Make the topic trigger the lambda
1. Go to ```Lambda```
2. Click on your lambda's name
3. Click on ```Add trigger```
4. Select ```SNS```
5. Select the one you just created
6. Levae default values
7. Click ```Add```

### Test it
1. Click on the topic name
2. Click on ```Publish messages```
3. Write something in the body
4. Click ```Publish```
5. Go back to your lambda
6. Click on ```Monitoring```
7. Click on ```See registers in CloudWatch```
8. Click on the last registry sequence
9. You should see an ```info``` message with what you wrote in the body.

## CLI
### Create a topic
1. Run: ```aws sns create-topic --name MyFunctionName```
2. Store the ARN.

### Make the topic trigger the lambda
1. Get the function data: ```aws lambda get-function --function-name MyFunctionName```
2. Store the ```FunctionArn```
3. Grant permissions for SNS: ```aws lambda add-permission --function-name MyFunctionName --source-arn arn:aws:lambda:LOCATION:111111:function:MyFunctionName --statement-id MyFunctionName --action "lambda:InvokeFunction" --principal sns.amazonaws.com```
4. Create subscription: ```aws sns subscribe --protocol lambda --topic-arn arn:aws:sns:LOCATION:111111:MyFunctionName --notification-endpoint arn:aws:lambda:LOCATION:1111111111:function:MyFunctionName```

### Test it
3. Run: ```aws logs describe-log-streams --log-group-name /aws/lambda/MyFunctionName```
4. Store the log stream name
5. Run: ```aws logs get-log-events --log-group-name /aws/lambda/MyFunctionName --log-stream-name 'LOG-STREAM-NAME'```
6. Go back to the other window
7. The output should look like:
```
PS > aws logs get-log-events --log-group-name /aws/lambda/MyFunctionName --log-stream-name '2022/12/01/[$LATEST]916b6ea1f3be44b1a41e63179711e2e0'
{
    "events": [
        {
            "timestamp": 1669894355456,
            "message": "START RequestId: 0c6b03cb-0ccd-47a0-9d84-4eedc1d1164c Version: $LATEST\n",
            "ingestionTime": 1669894364193
        },
        {
            "timestamp": 1669894355831,
            "message": "2022-12-01T11:32:35.791Z\t0c6b03cb-0ccd-47a0-9d84-4eedc1d1164c\tinfo\tProcessed record \"Hello world from SNS topic in AWS Console\"\n",
            "ingestionTime": 1669894364193
        },
        {
            "timestamp": 1669894355894,
            "message": "END RequestId: 0c6b03cb-0ccd-47a0-9d84-4eedc1d1164c\n",
            "ingestionTime": 1669894364193
        },
        {
            "timestamp": 1669894355894,
            "message": "REPORT RequestId: 0c6b03cb-0ccd-47a0-9d84-4eedc1d1164c\tDuration: 437.68 ms\tBilled Duration: 438 ms\tMemory Size: 256 MB\tMax Memory Used: 61 MB\tInit Duration: 223.71 ms\t\n",
            "ingestionTime": 1669894364193
        }
    ],
    "nextForwardToken": "f/37239888539256508088188017475800657595825812153553715203/s",
    "nextBackwardToken": "b/37239888529488781691231604539808012992405829813934292992/s"
}
```


# AWS Lambda Simple SNS Function Project

This starter project consists of:
* Function.cs - class file containing a class with a single function handler method
* aws-lambda-tools-defaults.json - default argument settings for use with Visual Studio and command line deployment tools for AWS

You may also have a test project depending on the options selected.

The generated function handler responds to events on an Amazon SNS service.

After deploying your function you must configure an Amazon SNS service as an event source to trigger your Lambda function.

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
    cd "LambdaReceivesSNSEvent/test/LambdaReceivesSNSEvent.Tests"
    dotnet test
```

Deploy function to AWS Lambda
```
    cd "LambdaReceivesSNSEvent/src/LambdaReceivesSNSEvent"
    dotnet lambda deploy-function
```
