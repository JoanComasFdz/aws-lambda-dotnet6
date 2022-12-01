# Description
This is an actual example of how a .NET6 minimal api application can publish an event into an SNS topic

# How to create a lambda that publishes an SNS Event
1. Use the project [LambdasWithMinimalApi](../../../LambdaWithMinimalApi/src/LambdaWithMinimalApi/) to know more about how to create one yourself.
2. Get the role name: ```aws lambda get-function --function-name MyFunctionName-AspNetCoreFuntion-111111```
3. The role name, not the full ARN, for example: ```MyFunctionName-AspNetCoreFunctionRole-1111XXXX```
4. Allow the lambda to publish in SNS: ```aws iam attach-role-policy --policy-arn arn:aws:iam::aws:policy/AmazonSNSFullAccess --role-name MyFunctionName-AspNetCoreFunctionRole-1111XXXX```
5. Add the SNS Topic ARN as env variable: ```aws lambda update-function-configuration --function-name MyFunctionName-AspNetCoreFunction-9ygcHy2A07zJ --environment "Variables={AWS_SNS_TOPICARN=arn:aws:sns:LOCATION:1111111:MyFunctionName}"```
