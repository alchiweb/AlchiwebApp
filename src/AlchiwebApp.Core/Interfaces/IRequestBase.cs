namespace AlchiwebApp.Core.Interfaces;

public interface IRequestBase<TId> where TId : struct, IEquatable<TId>
{
    TId Id { get; }
}
