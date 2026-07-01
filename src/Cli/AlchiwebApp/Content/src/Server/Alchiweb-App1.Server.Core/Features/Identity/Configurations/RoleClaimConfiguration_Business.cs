using Alchiweb-App1.Core.Infrastructure.Services;
using Alchiweb-App1.Server.Core.Features.Identity.Models;

namespace Alchiweb-App1.Server.Core.Features.Identity.Configurations;

public partial class RoleClaimConfiguration_Business : IEntityTypeConfiguration<RoleClaim>
{
    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        var id = 1000;

        // Assign non admin features to demo role
        foreach (var roleId in new[]
            {
            // [BusinessCustomCode]
            //  Add custom roles like the SpecialRole with the GUID defined in RoleConfiguration_Business:
            "3F921DF3-9177-4342-B0BC-543B865E371A",
        }.Select(s => Guid.Parse(s)))
        {
            foreach (var feature in AppFeatures.GetAll()
                .Where(f => f.Group != typeof(AppFeatures.System)
                         && f.Group != typeof(AppFeatures.Management)))
            {
                builder.HasData(new
                {
                    Id = id++,
                    ClaimType = AppClaimTypes.FEATURES,
                    ClaimValue = feature.Value,
                    RoleId = roleId
                });
            }
        }
    }
}
