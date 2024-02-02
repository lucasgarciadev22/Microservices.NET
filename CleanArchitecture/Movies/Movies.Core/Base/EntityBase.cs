namespace Movies.Core.Base;

public class EntityBase<TId> : IEntityBase<TId>
{
    public TId Id { get; protected set; }
}
