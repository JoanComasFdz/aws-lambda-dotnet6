# Description
This is an actual example of how a .NET6 minimal api application can publish an event into an SNS topic

# How to create a lambda that publishes an SNS Event
1. Use the project [LambdasWithMinimalApi](../../../LambdaWithMinimalApi/src/LambdaWithMinimalApi/) to know more about how to create one yourself.
2. Get the role name: ```aws lambda get-function --function-name MyFunctionName-AspNetCoreFuntion-111111```
3. The role name, not the full ARN, for example: ```MyFunctionName-AspNetCoreFunctionRole-1111XXXX```
4. Allow the lambda to publish in SNS: ```aws iam attach-role-policy --policy-arn arn:aws:iam::aws:policy/AmazonSNSFullAccess --role-name MyFunctionName-AspNetCoreFunctionRole-1111XXXX```
5. Add the SNS Topic ARN as env variable: ```aws lambda update-function-configuration --function-name MyFunctionName-AspNetCoreFunction-9ygcHy2A07zJ --environment "Variables={AWS_SNS_TOPICARN=arn:aws:sns:LOCATION:1111111:MyFunctionName}"```

# Add observability via AWS X-Ray
1. Enable tracing for the lambda: ```aws lambda update-function-configuration --function-name MyFunctionName-AspNetCoreFunction-111XXX --tracing-config Mode=Active```
2. Enable tracing for API Gatway Prod stage: ```aws apigateway update-stage --rest-api-id 7t4adodhpb --stage-name Prod --patch-operations op=replace,path=/tracingEnabled,value=true```
2. Go to your project path
3. Run: ```dotnet add package AWSXRayRecorder```
4. Run: ```dotnet add package AWSXRayRecorder.Handlers.AwsSdk```
5. Add this code after ```builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);```
```
#if !DEBUG
AWSSDKHandler.RegisterXRayForAllServices();
#endif
```

# Performance
Here are some ballpark values observed while manually testing:
- Cold start: 4.6s
- Next call: 600ms
- Next calls: 200ms - 600ms