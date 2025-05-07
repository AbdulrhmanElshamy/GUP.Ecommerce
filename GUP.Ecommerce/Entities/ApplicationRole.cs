using Microsoft.AspNetCore.Identity;

namespace GUP.Ecommerce.Entities;

public class ApplicationRole : IdentityRole
{
    public ApplicationRole()
    {
        Id = Guid.NewGuid().ToString();
    }

    public bool IsDefault { get; set; }
    public bool IsDeleted { get; set; }
}