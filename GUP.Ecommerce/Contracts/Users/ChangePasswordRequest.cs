namespace GUP.Ecommerce.Contracts.Users;

public record ChangePasswordRequest(
    string CurrentPassword,
    string NewPassword
);