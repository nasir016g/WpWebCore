using Microsoft.Identity.Client;
using Nsr.ActivityLogs.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Retrieve the connection string
var appConfig = builder.Configuration.GetSection("AppConfig");
string connectionString = appConfig.GetValue<string>("Connection");

// Load configuration from Azure App Configuration
builder.Configuration.AddAzureAppConfiguration(connectionString);

// Add services to the container.
builder.Services.AddDbContexts(builder.Configuration);
builder.Services.AddServices();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
ServiceCollectionExtensions.Migrate(app);
app.UseAuthorization();

app.MapControllers();

app.Run();
