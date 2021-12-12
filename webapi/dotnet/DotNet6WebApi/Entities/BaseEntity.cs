public abstract class BaseEntity
{
    public Guid Id { get; protected set; }

    public BaseEntity()
    {
        this.Id = Guid.NewGuid();
    }
}
