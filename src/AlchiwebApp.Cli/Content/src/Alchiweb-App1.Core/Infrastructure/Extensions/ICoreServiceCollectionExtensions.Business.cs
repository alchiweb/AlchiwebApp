namespace Microsoft.Extensions.DependencyInjection;

public static partial class ICoreServiceCollectionExtensions
{
    public static void AddTypedHttpClients(this IServiceCollection services, bool manualAdding)
    {
        if (manualAdding)
        {
            services.TryAddTransient<Alchiweb-App1.Core.Features.Attachments.IAttachmentController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_Attachments_IAttachmentController>();
            services.TryAddTransient<Alchiweb-App1.Core.Features.Diagnostic.IDiagnosticController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_Diagnostic_IDiagnosticController>();
            services.TryAddTransient<Alchiweb-App1.Core.Features.Identity.IIdentityController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_Identity_IIdentityController>();
            services.TryAddTransient<Alchiweb-App1.Core.Features.Identity.OAuth.IOAuthClientManagementController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_Identity_OAuth_IOAuthClientManagementController>();
            services.TryAddTransient<Alchiweb-App1.Core.Features.Identity.OAuth.IOAuthController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_Identity_OAuth_IOAuthController>();
            services.TryAddTransient<Alchiweb-App1.Core.Features.Identity.IRoleManagementController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_Identity_IRoleManagementController>();
            services.TryAddTransient<Alchiweb-App1.Core.Features.Identity.IUserController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_Identity_IUserController>();
            services.TryAddTransient<Alchiweb-App1.Core.Features.Identity.IUserManagementController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_Identity_IUserManagementController>();
            services.TryAddTransient<Alchiweb-App1.Core.Features.PushNotification.IPushNotificationController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_PushNotification_IPushNotificationController>();
            services.TryAddTransient<Alchiweb-App1.Core.Features.Statistics.IStatisticsController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_Statistics_IStatisticsController>();
#if ALCHIWEBAPP_CHATBOT
            services.TryAddTransient<Alchiweb-App1.Core.Features.Chatbot.IChatbotController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_Chatbot_IChatbotController>();
#endif
#if ALCHIWEBAPP_TENANTS
            services.TryAddTransient<Alchiweb-App1.Core.Features.Tenants.ITenantController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_Tenants_ITenantController>();
            services.TryAddTransient<Alchiweb-App1.Core.Features.Tenants.ITenantManagementController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_Tenants_ITenantManagementController>();
#endif
#if ALCHIWEBAPP_MODULE_ADMIN
            services.TryAddTransient<Alchiweb-App1.Core.Features.Dashboard.IDashboardController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_Dashboard_IDashboardController>();
            services.TryAddTransient<Alchiweb-App1.Core.Features.Categories.ICategoryController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_Categories_ICategoryController>();
            services.TryAddTransient<Alchiweb-App1.Core.Features.Products.IProductController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_Products_IProductController>();
#endif
#if ALCHIWEBAPP_MODULE_SALES
            services.TryAddTransient<Alchiweb-App1.Core.Features.Products.IProductViewController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_Products_IProductViewController>();
#endif
#if ALCHIWEBAPP_SAMPLE_MINIMALAPI
            services.TryAddTransient<Alchiweb-App1.Core.Features.MinimalApiSample.IMinimalApiSampleController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_MinimalApiSample_IMinimalApiSampleController>();
#endif
#if ALCHIWEBAPP_SAMPLE_TODO
            services.TryAddTransient<Alchiweb-App1.Core.Features.Todo.ITodoItemController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_Todo_ITodoItemController>();
#endif
        }
        else
        {
            services.AddTypedHttpClients();
        }
    }
}
