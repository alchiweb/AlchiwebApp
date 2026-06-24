#if ALCHIWEBAPP
using Ardalis.Specification.EntityFrameworkCore.SharedKernel;
#endif

namespace Alchiweb-App1.Server.Core.Features.Identity.Models;

public partial class User : IdentityUser<Guid>
#if ALCHIWEBAPP
    , IEntityBase<User, Guid>, IApplicationUser
#endif
{
    // [BusinessCustomCode]
    //  Add custom properties like:
    //public DateTimeOffset CreatedAt { get; set; }
    //public DateTimeOffset? LastConnection { get; set; }
    //[PersonalData]
    //public Guid? MemberId { get; set; }

#if ALCHIWEBAPP_USER_ROLE
    public RoleEnum Role
    {
        get
        {
            try
            {
                return Enum.Parse<RoleEnum>(string.Join(',', Roles.Select(ur => ur.Role?.Name ?? "").ToArray()));
            }
            catch { return 0; }
        }
        set { }
    }
#endif
}
