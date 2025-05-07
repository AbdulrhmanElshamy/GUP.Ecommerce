namespace GUP.Ecommerce.Contracts.Users;

public record UpdateUserRequest(
    string FirstName,
    string LastName,
    string Email,
    string phone,
    string ShippingAddress
);