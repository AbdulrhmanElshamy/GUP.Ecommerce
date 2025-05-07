namespace GUP.Ecommerce.Contracts.Users;

public record UpdateProfileRequest(
    string FirstName,
    string LastName
);