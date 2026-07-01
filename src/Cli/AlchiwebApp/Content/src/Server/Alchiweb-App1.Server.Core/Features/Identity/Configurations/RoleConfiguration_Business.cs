using Alchiweb-App1.Core.Infrastructure.Services;
using Alchiweb-App1.Server.Core.Features.Identity.Models;

namespace Alchiweb-App1.Server.Core.Features.Identity.Configurations;

public partial class RoleConfiguration_Business : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        // [BusinessCustomCode]
        //  Add custom roles like:
        builder.HasData(new Role { Id = Guid.Parse("3F921DF3-9177-4342-B0BC-543B865E371A"), Name = AppRoles.SpecialManager, NormalizedName = AppRoles.SpecialManager.ToUpperInvariant(), ConcurrencyStamp = "3F921DF3-9177-4342-B0BC-543B865E371A" });
    }
}
