namespace Movies.Core.Entities.Base;

public class EntityBase<TId> : IEntityBase<TId>
{
    public TId Id { get; protected set; }
}
