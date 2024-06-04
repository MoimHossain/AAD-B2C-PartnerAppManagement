


using B2CBackend;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<ConfigReader>();
builder.Services.AddSingleton<HttpRequestReader>();
builder.Services.AddSingleton<B2CAppService>();
builder.Services.AddSingleton<ApimService>();
builder.Logging.AddSimpleConsole(options =>
{
    options.IncludeScopes = true;
    options.SingleLine = true;
    options.TimestampFormat = "hh:mm:ss ";
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

var appRegApi = app.MapGroup("/api");

appRegApi.MapGet("/list", async (HttpRequest request, 
    [FromServices] HttpRequestReader requestReader,
    [FromServices] B2CAppService b2CAppService) =>
{
    var context = requestReader.GetInvocationContext(request);
    var appRegs = await b2CAppService.GetAppRegsAsync(context);

    return appRegs;
}).WithOpenApi();

appRegApi.MapPost("/generate", async (
    [FromBody] GeneratePayload payload,
    [FromServices] HttpRequestReader requestReader,
    [FromServices] B2CAppService b2CAppService,
    HttpRequest request) =>
{
    var context = requestReader.GetInvocationContext(request);
    var appRegs = await b2CAppService.NewAppRegsAsync(context, payload);

    return appRegs;
}).WithOpenApi();

app.Run();

