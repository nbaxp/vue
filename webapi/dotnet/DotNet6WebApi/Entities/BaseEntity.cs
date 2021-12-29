public abstract class BaseEntity
{
    public Guid Id { get; protected set; }
    public string ConcurrencyStamp { get; set; } = null!;
    public BaseEntity()
    {
        this.Id = Guid.NewGuid();
    }
}
