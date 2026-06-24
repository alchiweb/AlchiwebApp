namespace Alchiweb-App1.Server.Core.Features.Identity.Models;

public partial class Role : IdentityRole<Guid>
{
#if ALCHIWEBAPP_USER_ROLE
    public Role() : base() { }
    public Role(string roleName) : base(roleName) { }
#endif
}

