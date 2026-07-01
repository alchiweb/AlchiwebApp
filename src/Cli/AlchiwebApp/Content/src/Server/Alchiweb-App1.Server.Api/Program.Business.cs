
namespace Alchiweb-App1.Server.Api;

public static partial class Program
{
    /// <summary>
    /// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-9.0#middleware-order
    /// </summary>
    private static void ConfigureBusinessMiddlewares(this WebApplication app)
    {
        // [BusinessCustomCode]
        //  Add here custom middlewares
        //  and add minimal API endpoints, like:
        //var validationGroup = app.MapGroup("/");
        //validationGroup
        //    .UseMyDataEndpoints();
    }
}
