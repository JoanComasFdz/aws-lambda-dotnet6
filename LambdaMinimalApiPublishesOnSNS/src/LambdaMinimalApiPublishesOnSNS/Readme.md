# Description
This is an actual example of how a .NET6 minimal api application can publish an event into an SNS topic.

Use the project [LambdasWithMinimalApi](../../../LambdaWithMinimalApi/src/LambdaWithMinimalApi/) to know more about how to create one yourself.

Follow the steps in [eployment.ipynb](./deployment.ipynb) to deploy and test the lambda.

# Performance
Here are some ballpark values observed while manually testing:
- Cold start: ~4.6s
- Next call: ~600ms
- Next calls: ~200ms - ~600ms