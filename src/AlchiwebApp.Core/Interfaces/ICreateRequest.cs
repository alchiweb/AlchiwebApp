namespace AlchiwebApp.Core.Interfaces;

public interface ICreateRequest<TEntityDto, TId>
    where TEntityDto : IEntityDto<TId>
    where TId : struct, IEquatable<TId>
{
    
}
