using Alchiweb-App1.Server.Core.Features.Identity.Models;

namespace Alchiweb-App1.Server.Core.Features.Identity.Configurations;

public partial class UserConfiguration_Business : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        #region [AlchiwebApp] Added
#if ALCHIWEBAPP_USER_ROLE
        builder.Navigation(u => u.Roles)
            .AutoInclude();
#endif
#endregion
    }
}
