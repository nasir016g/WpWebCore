using Azure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Nsr.Common.Core;
using Nsr.Common.Service.Extensions;
using Nsr.RestClient.Extensions;
using Nsr.Work.Web.Controllers;
using Nsr.Work.Web.Extensions;
using Nsr.Work.Web.Infrastructure.Mapper;

var builder = WebApplication.CreateBuilder(args);


// Retrieve the connection string
var appConfig = builder.Configuration.GetSection("AppConfig");
string connectionString = appConfig.GetValue<string>("Connection");

// Load configuration from Azure App Configuration
//builder.Configuration.AddAzureAppConfiguration(connectionString);

builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(connectionString)
    .ConfigureKeyVault(x =>
    {
        x.SetCredential(new DefaultAzureCredential());
    });
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddDbContexts(builder.Configuration);
builder.Services.AddServices();
builder.Services.AddActivityLogRestClients(builder.Configuration);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddNsrCommon(builder.Configuration);
builder.Services.AddAutoMapper(typeof(WpBaseController));
AutoMapperConfiguration.Init();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Wp.Resumes.WebApi", Version = "v1" });
});

var app = builder.Build();

ServiceLocator.Instance = app.Services;
Nsr.Common.Service.Extensions.ServiceCollectionExtensions.UseNsrCommon(app);
Nsr.Work.Web.Extensions.ServiceCollectionExtensions.Migrate(app);
//if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseSession();
app.MapControllers();

app.Run();


