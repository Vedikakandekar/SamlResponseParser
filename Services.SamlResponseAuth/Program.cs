using Serilog;
using Services.SamlResponseAuth.Services;
using Services.SamlResponseAuth.Services.Contracts;
using Services.SamlResponseAuth.Utility;
using Services.SamlResponseAuth.Utility.Contracts;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt")
    .Enrich.FromLogContext() 
    .CreateLogger();

builder.Host.UseSerilog();

builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<SamlXPathSettings>(builder.Configuration.GetSection("SAML"));


builder.Services.AddSingleton<IExceptionMapper, ExceptionMapperService>();
builder.Services.AddSingleton<IXmlDocumentLoader, XmlDocumentLoader>();
builder.Services.AddSingleton<IXmlNamespaceManagerFactory, XmlNamespaceManagerFactory>();
builder.Services.AddTransient<ISamlAuthService, SamlAuthService>();
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
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
