using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container, necessary even if no controllers exist.
builder.Services.AddControllers();

// Add AWS Lambda support. When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
// package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Welcome to running ASP.NET Core Minimal API on AWS Lambda");

app.MapGet("/calculator/add/{x}/{y}", (int x, int y, [FromServices] ILogger<Program> logger) =>
{
    logger.LogInformation($"{x} plus {y} is {x + y}");
    return x + y;
});

app.MapGet("/calculator/subtract/{x}/{y}", (int x, int y, [FromServices] ILogger<Program> logger) =>
{
    logger.LogInformation($"{x} subtract {y} is {x + y}");
    return x - y;
});

app.MapGet("/calculator/multiply/{x}/{y}", (int x, int y, [FromServices] ILogger<Program> logger) =>
{
    logger.LogInformation($"{x} multiply {y} is {x + y}");
    return x * y;
});

app.MapGet("/calculator/divide/{x}/{y}", (int x, int y, [FromServices] ILogger<Program> logger) =>
{
    logger.LogInformation($"{x} divide {y} is {x + y}");
    return x / y;
});

app.Run();
