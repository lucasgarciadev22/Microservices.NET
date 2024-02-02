namespace Movies.Core.Base;

public interface IEntityBase<TId>
{
    TId Id { get; }
}
