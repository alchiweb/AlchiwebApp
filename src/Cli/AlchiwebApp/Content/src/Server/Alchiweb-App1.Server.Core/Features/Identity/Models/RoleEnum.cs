namespace Alchiweb-App1.Server.Core.Features.Identity.Models;

#if ALCHIWEBAPP_USER_ROLE
[Flags]
public enum RoleEnum
{
    AppAdministrator        = 1 << 0,
}
#endif
