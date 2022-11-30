# Description
This is the same as the [LambdaWithMinimalApi](..\..\LambdaWithMinimalApi) so follow the instructions in that readme

Here, the controller is removed and the methods are written directly into the ```Program.cs```, becoming a real minimal api in one single file.

Unfortunately, an important limitation are filters, which are not supported in .NET6. They are available in .NET 7 though.

---

Omitted *original* ```Readme.md``` file created by the template, since it is the same as in LambdaWithMinimalApi.