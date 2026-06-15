namespace Microsoft.Extensions.Hosting;

public static partial class WebApplicationBuilderExtensions
{
    public static IServiceCollection ConfigureAlchiwebAppServerServices(this IServiceCollection services)
    {
        // [BusinessCustomCode]
        //  Add services like:
        //services.AddSingleton<DtoMapper>();
        //services.AddScoped<IFileService, FileService>();
        //if (!AppPlatform.IsBlazorHybridOrBrowser)
        //{
        //    services.AddScoped<ICryptoService, CryptoService>();
        //    services.ConfigureBusinessApplicationApiServices();
        //}
        return services;
    }

    private static IServiceCollection ConfigureBusinessApplicationApiServices(this IServiceCollection services)
    {
        // [BusinessCustomCode]
        //  Add services like:
        // services.AddScoped<ICustomApiService, CustomeApiService>();
        return services;
    }

    private static TBuilder ConfigureBusinessInfrastructure<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        // [BusinessCustomCode]
        //  Add AddDbContextFactory if needed

        return builder;
    }

}
