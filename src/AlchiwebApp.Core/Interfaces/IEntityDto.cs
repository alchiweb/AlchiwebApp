using System;
using System.Collections.Generic;
using System.Text;

namespace AlchiwebApp.Core.Interfaces
{
    public interface IEntityDto<TId> where TId : struct, IEquatable<TId>
    {
        TId Id { get; }
    }
    //public interface IEntityDto : DtoBase<Guid>
    //{
    //}
}
