namespace Alchiweb-App1.Client.Core.Components.Layout;

public partial class MainLayout
{
#if ALCHIWEBAPP
    [AutoInject] protected IStringLocalizer<Resources.ClientI18n> localizerAlchiweb-App1 = default!;
    [AutoInject] protected IStringLocalizer<I18n> localizerAlchiweb-App1Core = default!;
    private async Task SetNavPanelItemsBusiness(ClaimsPrincipal authUser, List<BitNavItem> navPanelItems)
    {
        // Add here specific menu items (like before calling this method)
    }
#endif
}
