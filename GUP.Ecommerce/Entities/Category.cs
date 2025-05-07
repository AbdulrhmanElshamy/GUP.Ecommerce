namespace GUP.Ecommerce.Entities;

public class Category : AuditableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    public string NameArabic { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; }
}


