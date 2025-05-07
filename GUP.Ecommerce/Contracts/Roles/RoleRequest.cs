namespace GUP.Ecommerce.Contracts.Roles;

public record RoleRequest(
    string Name,
    IList<string> Permissions
);