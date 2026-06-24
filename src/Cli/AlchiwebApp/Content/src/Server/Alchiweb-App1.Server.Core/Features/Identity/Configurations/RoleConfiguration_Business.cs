using Alchiweb-App1.Core.Infrastructure.Services;
using Alchiweb-App1.Server.Core.Features.Identity.Models;

namespace Alchiweb-App1.Server.Core.Features.Identity.Configurations;

public partial class RoleConfiguration_Business : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasData(new Role { Id = Guid.Parse("D16FB616-0B66-4E3A-B8EF-9B5F42F33D59"), Name = AppRoles.Teacher, NormalizedName = AppRoles.Teacher.ToUpperInvariant(), ConcurrencyStamp = "D16FB616-0B66-4E3A-B8EF-9B5F42F33D59" });
        builder.HasData(new Role { Id = Guid.Parse("14A74BE8-EC5D-4669-955D-7E907C0FFFB2"), Name = AppRoles.PedagogicManager, NormalizedName = AppRoles.PedagogicManager.ToUpperInvariant(), ConcurrencyStamp = "14A74BE8-EC5D-4669-955D-7E907C0FFFB2" });
        builder.HasData(new Role { Id = Guid.Parse("7B2F6C87-1190-4905-9544-A1392D001BBC"), Name = AppRoles.AlumniManager, NormalizedName = AppRoles.AlumniManager.ToUpperInvariant(), ConcurrencyStamp = "7B2F6C87-1190-4905-9544-A1392D001BBC" });
    }
}
