using Alchiweb-App1.Core.Infrastructure.Services;
using Alchiweb-App1.Server.Core.Features.Identity.Models;

namespace Alchiweb-App1.Server.Core.Features.Identity.Configurations;

public partial class RoleClaimConfiguration_Business : IEntityTypeConfiguration<RoleClaim>
{
    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        // Assign non admin features to demo role
        foreach (var roleId in new[] {
            "D16FB616-0B66-4E3A-B8EF-9B5F42F33D59",
            "14A74BE8-EC5D-4669-955D-7E907C0FFFB2",
            "7B2F6C87-1190-4905-9544-A1392D001BBC"
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
