public abstract class BaseEntity
{
    public virtual string Id { get; protected set; }
    public BaseEntity()
    {
        this.Id = Guid.NewGuid().ToString();
    }

    public BaseEntity(string id)
    {
        this.Id = id;
    }
}
