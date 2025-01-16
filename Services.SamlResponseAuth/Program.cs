using Serilog;
using Services.SamlResponseAuth.Services;
using Services.SamlResponseAuth.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console() 
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day) 
    .Enrich.FromLogContext() 
    .CreateLogger();

builder.Host.UseSerilog();

builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ISamlAuthService,SamlAuthService>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
