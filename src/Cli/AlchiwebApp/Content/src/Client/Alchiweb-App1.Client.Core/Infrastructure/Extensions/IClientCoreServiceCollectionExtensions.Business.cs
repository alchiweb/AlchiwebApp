using System.Diagnostics.CodeAnalysis;
using Alchiweb-App1.Client.Core;
using AlchiwebApp.Client.Core.AntD;
using Alchiweb-App1.Client.Core.Infrastructure.Services.HttpMessageHandlers;
using Alchiweb-App1.Core.Features.Security;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class IClientCoreServiceCollectionExtensions
{
    public static IServiceCollection AddAlchiwebAppServices(this IServiceCollection services)
    {
        // For Ant Design Blazor
        services.AddAntDesign();
        services.AddSingleton<AntDComponentsConfiguration>(); // custom table localization for french language

        // And Design Blazor toasted messages
        services.AddScoped<IToastService, ToastService>();

        services.AddScoped<IMediaService, MediaService>();
        if (AppPlatform.IsBlazorHybridOrBrowser)
        {
            services.AddScoped<ICryptoService, CryptoService>();
        }

        return services;
    }
}
