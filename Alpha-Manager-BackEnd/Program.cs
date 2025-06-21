using Alpha.Application.Interfaces;
using Alpha.Application.Queries;
using Alpha.Domain.Interfaces;
using Alpha.Persistences.Context;
using Alpha.Persistences.FakeStoreService;
using Alpha.Persistences.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var configuration = builder.Configuration;
var corsPolicyName = "AllowViteDevServer";

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    options.EnableSensitiveDataLogging();
});


AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
NpgsqlConnection.GlobalTypeMapper.UseNetTopologySuite();


builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<Program>();
    cfg.RegisterServicesFromAssemblyContaining<GetProductsQueryHandler>();
});


builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddHttpClient<IFakeStoreService, FakeStoreService>(client =>
{
    client.BaseAddress = new Uri("https://fakestoreapi.com/");
});


builder.Services.AddHealthChecks();


builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName,
        policy => policy
            .WithOrigins("http://localhost:5173", "https://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod());
});


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    })
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });


builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "ALPHA Product API",
        Version = "v1",
        Description = "Gerenciamento de produtos com integração Fake Store"
    });
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ALPHA Product API V1");
});

app.UseAuthorization();
app.UseCors(corsPolicyName);
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
