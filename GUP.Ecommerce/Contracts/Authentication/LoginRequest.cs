namespace GUP.Ecommerce.Contracts.Authentication;

public record LoginRequest(
    string Email,
    string Password
);