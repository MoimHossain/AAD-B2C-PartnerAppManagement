


using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

var appRegApi = app.MapGroup("/api");

appRegApi.MapGet("/list", async () =>
{
    await Task.Delay(100);
    var appregs = new List<AppReg>
    {
        new AppReg("123", "456", "App1", "xxx", "tenant-2323", DateTimeOffset.Now, DateTimeOffset.Now.AddHours(1)),
        new AppReg("789", "101", "App2", "xxx", "tenant-4554", DateTimeOffset.Now, DateTimeOffset.Now.AddHours(1))
    };
    return appregs;
})
.WithOpenApi();

appRegApi.MapPost("/generate", async ([FromBody] GeneratePayload payload) =>
{
    await Task.Delay(100);

    var appregs = new List<AppReg>
    {
        new AppReg("123", "456", "App1", "xxx", "tenant-2323", DateTimeOffset.Now, DateTimeOffset.Now.AddHours(1)),
        new AppReg("789", "101", "App2", "xxx", "tenant-4554", DateTimeOffset.Now, DateTimeOffset.Now.AddHours(1))
    };
 

    var appReg = new AppReg("123", "456", payload.DisplayName, "xxx", "tenant-2323", DateTimeOffset.Now, DateTimeOffset.Now.AddHours(1));
    appregs.Add(appReg);

    return appregs;
})
.WithOpenApi();

app.Run();

public record GeneratePayload(string DisplayName);

public record AppReg(
    string ClientId, 
    string SecretId, 
    string DisplayName,
    string ClientSecret,
    string TenantId,
    DateTimeOffset StartDateTime, 
    DateTimeOffset EndDateTime);