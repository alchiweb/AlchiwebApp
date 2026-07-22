using Alchiweb-App1.Server.Core.Features.Identity.Models;

namespace Alchiweb-App1.Server.Core.Features.Identity.Configurations;

public partial class UserRoleConfiguration_Business : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
#if ALCHIWEBAPP_USER_ROLE
        #region [AlchiwebApp] Added
        builder.Navigation(ur => ur.Role)
            .AutoInclude();
        #endregion
#endif
    }
}
