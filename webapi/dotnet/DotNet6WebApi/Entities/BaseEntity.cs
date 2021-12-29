﻿public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public BaseEntity()
    {
        this.Id = Guid.NewGuid();
    }
}
