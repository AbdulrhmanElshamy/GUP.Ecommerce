﻿using Microsoft.AspNetCore.Mvc;
using GUP.Ecommerce.Abstractions;
using GUP.Ecommerce.Contracts.Authentication;
using GUP.Ecommerce.UserServices.Services;

namespace GUP.Ecommerce.Controllers;

[Route("[controller]")]
public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly ILogger<AuthController> _logger = logger;

    /// <summary>
    /// Allow users to get Jwt token
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Return Jwt token if credentials were valid</returns>

    [HttpPost("")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Logging with email: {email} and password: {password}", request.Email, request.Password);

        var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

        return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem();
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem();
    }

    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    //[HttpPost("register")]
    //public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    //{
    //    var result = await _authService.RegisterAsync(request, cancellationToken);

    //    return result.IsSuccess ? Ok() : result.ToProblem();
    //}

    //[HttpPost("confirm-email")]
    //public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request, CancellationToken cancellationToken)
    //{
    //    var result = await _authService.ConfirmEmailAsync(request);

    //    return result.IsSuccess ? Ok() : result.ToProblem();
    //}

    //[HttpPost("resend-confirmation-email")]
    //public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmailRequest request, CancellationToken cancellationToken)
    //{
    //    var result = await _authService.ResendConfirmationEmailAsync(request);

    //    return result.IsSuccess ? Ok() : result.ToProblem();
    //}

    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
    {
        var result = await _authService.SendResetPasswordCodeAsync(request.Email);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var result = await _authService.ResetPasswordAsync(request);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
}