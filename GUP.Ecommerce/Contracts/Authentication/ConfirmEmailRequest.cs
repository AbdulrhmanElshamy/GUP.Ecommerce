namespace GUP.Ecommerce.Contracts.Authentication;

public record ConfirmEmailRequest(
    string UserId,
    string Code
);