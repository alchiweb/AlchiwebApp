using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Alchiweb-App1.Core;
using Alchiweb-App1.Core.Features.Attachments;
using Alchiweb-App1.Core.Features.Categories;
using Alchiweb-App1.Core.Features.Chatbot;
using Alchiweb-App1.Core.Features.Dashboard;
using Alchiweb-App1.Core.Features.Diagnostic;
using Alchiweb-App1.Core.Features.Identity;
using Alchiweb-App1.Core.Features.MinimalApiSample;
using Alchiweb-App1.Core.Features.Products;
using Alchiweb-App1.Core.Features.PushNotification;
using Alchiweb-App1.Core.Features.Statistics;
using Alchiweb-App1.Core.Features.Todo;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ICoreServiceCollectionExtensions
{
    #region Added for [Alchiweb-App1]
    public static void AddBitPlatformTypedHttpClients(this IServiceCollection services)
    {
        services.TryAddTransient<IChatbotController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_Chatbot_IChatbotController>();
        services.TryAddTransient<IPushNotificationController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_PushNotification_IPushNotificationController>();
        services.TryAddTransient<IRoleManagementController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_Identity_IRoleManagementController>();
        services.TryAddTransient<IAttachmentController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_Attachments_IAttachmentController>();
        services.TryAddTransient<IStatisticsController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_Statistics_IStatisticsController>();
        //services.TryAddTransient<IMinimalApiController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_MinimalApiSample_IMinimalApiController>();
        services.TryAddTransient<IDiagnosticController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_Diagnostic_IDiagnosticController>();
        services.TryAddTransient<IUserManagementController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_Identity_IUserManagementController>();
        services.TryAddTransient<IUserController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_Identity_IUserController>();
        services.TryAddTransient<IIdentityController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_Identity_IIdentityController>();
        services.TryAddTransient<ICategoryController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_Categories_ICategoryController>();
        services.TryAddTransient<IProductController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_Products_IProductController>();
        services.TryAddTransient<IDashboardController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_Dashboard_IDashboardController>();
        services.TryAddTransient<ITodoItemController, IHttpClientServiceCollectionExtensions.Alchiweb-App1_Core_Features_Todo_ITodoItemController>();
    }
    #endregion
}
