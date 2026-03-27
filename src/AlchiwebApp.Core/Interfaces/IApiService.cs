namespace AlchiwebApp.Core.Interfaces;

public interface IApiService<TEntityDto, TId>
    where TId : struct, IEquatable<TId>
    where TEntityDto : IEntityDto<TId>
{

}
