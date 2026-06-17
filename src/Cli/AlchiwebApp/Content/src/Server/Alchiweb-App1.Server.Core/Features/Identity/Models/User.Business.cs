// [BusinessCustomCode]
//  Add usings like:
//using Alchiweb-App1.Server.Domain.Users;
//using Ardalis.Specification.EntityFrameworkCore.SharedKernel;

namespace Alchiweb-App1.Server.Core.Features.Identity.Models;

public partial class User : IdentityUser<Guid>
    // [BusinessCustomCode]
    //  Add interfaces like:
    //, IEntityBase<User, Guid>, IApplicationUser
{
    // [BusinessCustomCode]
    //  Add custom properties like:
    //public DateTimeOffset CreatedAt { get; set; }
    //public DateTimeOffset? LastConnection { get; set; }
    //[PersonalData]
    //public Guid? MemberId { get; set; }
    //public RoleEnum Role
    //{
    //    get
    //    {
    //        try
    //        {
    //            return Enum.Parse<RoleEnum>(string.Join(',', Roles.Select(ur => ur.Role?.Name ?? "").ToArray()));
    //        }
    //        catch { return 0; }
    //    }
    //    set { }
    //}
}
