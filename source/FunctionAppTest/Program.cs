using System.Reflection;
using Azure.Identity;
using Boxed.AspNetCore;
using FunctionAppTest.ExceptionHandler;
using FunctionAppTest.Data;
using FunctionAppTest.Options;
using FunctionAppTest.Profiles;
using FunctionAppTest.Repository;
using FunctionAppTest.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration((context, builder) =>
    {
        var config = builder
            .SetBasePath(context.HostingEnvironment.ContentRootPath)
            .AddJsonFile("settings.json", true, true)
            .AddJsonFile("local.settings.json", true, false)
            .AddEnvironmentVariables()
            .AddCommandLine(Environment.GetCommandLineArgs())
            .Build();

        if (context.HostingEnvironment.IsDevelopment() &&
            !string.IsNullOrEmpty(context.HostingEnvironment.ApplicationName))
        {
            builder.AddUserSecrets(Assembly.GetExecutingAssembly(), true);
        }
        var keyVaultUrl = builder.Build()["KeyVaultUrl"];
        if (!context.HostingEnvironment.IsDevelopment() && !string.IsNullOrEmpty(keyVaultUrl))
        {
            var keyVaultUri = new Uri(keyVaultUrl);
            var creds = new DefaultAzureCredential();
            builder.AddAzureKeyVault(keyVaultUri, creds);
        }
    })
    .ConfigureServices( (context, services) =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IProductItemRepository, ProductItemRepository>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IProductItemService, ProductItemService>();
        services.AddAutoMapper(typeof(ProductProfile).Assembly);
        services.AddDbContext<ProductCatalogueContext>(
            opt =>
                SqlServerDbContextOptionsExtensions
                    .UseSqlServer(opt, "Name=Options:ConnectionStrings"));
        services
            .ConfigureAndValidateSingleton<ConnectionStringOptions>(
                context.Configuration.GetSection(nameof(ApplicationOptions.Options))
            );
    })
    // .ConfigureFunctionsWorkerDefaults((context,builder) => {
    //     if (!context.HostingEnvironment.IsDevelopment() &&
    //         !string.IsNullOrEmpty(context.HostingEnvironment.ApplicationName))
    //     {
    //         builder.UseMiddleware<ExceptionHandlingMiddleware>();
    //     }
    // })
    .Build();

host.Run();
