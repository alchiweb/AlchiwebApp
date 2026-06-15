using Alchiweb-App1.Core.Features.Security;

namespace Microsoft.Extensions.Hosting;

public static partial class WebApplicationBuilderExtensions
{
    public static TBuilder AddServerInfraServices<TBuilder>(this TBuilder builder, bool withMinimalServices = false)
        where TBuilder : IHostApplicationBuilder
    {
        // if withMinimalServices is false -> standard BitPlatform behavior
        builder.AddServerSharedServices(withMinimalServices);
        // and 
        if (!withMinimalServices)
        {
            builder.Services.ConfigureAlchiwebAppServerServices();
        }
        // else just add Aspire default services

        // Configure optional business infrastructure
        builder.ConfigureBusinessInfrastructure();

        return builder;
    }


    public static TBuilder AddServerSharedServices<TBuilder>(this TBuilder builder, bool withMinimalServices)
        where TBuilder : IHostApplicationBuilder
    {
        // if withMinimalServices is false -> standard BitPlatform behavior
        if (!withMinimalServices)
        {
            return builder.AddServerSharedServices();
        }
        // else just add Aspire default services
        builder.AddServiceDefaults();

        return builder;
    }

}
