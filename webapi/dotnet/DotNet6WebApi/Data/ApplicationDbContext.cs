using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entityTypes = modelBuilder.Model.GetEntityTypes().ToList();
        foreach (var entity in entityTypes)
        {
            if (entity.GetProperties().Any(o => o.Name == "Id"))
            {
                modelBuilder.Entity(entity.Name).HasKey("Id");
                modelBuilder.Entity(entity.Name).Property("Id").ValueGeneratedNever();
            }
            if (entity.GetProperties().Any(o => o.Name == "RowVersion"))
            {
                modelBuilder.Entity(entity.Name).Property("RowVersion").IsConcurrencyToken().ValueGeneratedNever();
            }
            //if (entity.GetProperties().Any(o => o.Name == "Created"))
            //{
            //    modelBuilder.Entity(entity.Name).Property("Created").ValueGeneratedOnAdd();
            //}
            //if (entity.GetProperties().Any(o => o.Name == "Modified"))
            //{
            //    modelBuilder.Entity(entity.Name).Property("Modified").ValueGeneratedOnUpdate();
            //}
            //if (entity.GetProperties().Any(o => o.Name == "Parent") && entity.GetProperties().Any(o => o.Name == "Children"))
            //{
            //    modelBuilder.Entity(entity.Name).HasOne("Parent").WithMany("Children").HasForeignKey(new string[] { "ParentId" }).OnDelete(DeleteBehavior.SetNull);
            //    if (entity.GetProperties().Any(o => o.Name == "Name"))
            //    {
            //        modelBuilder.Entity(entity.Name).Property("Name").IsRequired();
            //    }
            //    if (entity.GetProperties().Any(o => o.Name == "Number"))
            //    {
            //        modelBuilder.Entity(entity.Name).Property("Number").IsRequired();
            //    }
            //}
            //modelBuilder.Entity(entity.Name).HasComment(entity.ClrType.GetDisplayName());
            //foreach (var prop in entity.GetProperties())
            //{
            //    modelBuilder.Entity(entity.ClrType).Property(prop.Name).HasComment(prop.PropertyInfo.GetDisplayName());
            //}
        }
        modelBuilder.Entity<User>();
    }
}
