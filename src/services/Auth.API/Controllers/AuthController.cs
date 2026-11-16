using ASPNET.BackEnd.Common.Base;
using Auth.API.Features.Users;
using Auth.API.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Auth.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : BaseApiController
	{
		private readonly TokenService _tokenservice;
		private readonly Serilog.ILogger _logger;
		//private readonly ISender _mediator;

		public AuthController(ISender sender, TokenService tokenService, Serilog.ILogger logger) : base(sender)
		{
			_tokenservice = tokenService;
			_logger = logger.ForContext<AuthController>();
			//_mediator = sender;
		}

		//public AuthController(TokenService tokenService, IMediator mediator)
		//{
		//	_tokenservice = tokenService;
		//	_mediator = mediator;
		//}
		[HttpPost("login")]
		public IActionResult Login([FromBody] Auth.API.Models.LoginRequest login)
		{
			try
			{
				if (login != null)
				{
					if (login.email != null && login.password != null && login.email == "admin" && login.password == "123")
					{
						var token = _tokenservice.GenerateToken(login.email);
						var refreshToken = _tokenservice.GenerateRefreshToken();
						var refreshTokenEntity = new RefreshToken
						{
							Token = refreshToken,
							UserId = login.email,
							ExpiryDate = DateTime.UtcNow.AddDays(7),
							CreatedAt = DateTime.UtcNow
						};

						//_context.RefreshTokens.Add(refreshTokenEntity);
						//await _context.SaveChangesAsync();

						var user = new { name = "vijay", role = "admin" };
						return Ok(new { token, user, refreshToken });
					}
				}

				_logger.Warning("Login failed for user {Email}: Invalid username or password", login?.email);
				return Unauthorized("Invalid username or password");
			}
			catch (Exception ex)
			{
				_logger.Error(
					ex,
					"An error occurred during login for user: {Email}. Exception: {ExceptionMessage}",
					login?.email,
					ex.Message);

				return StatusCode(StatusCodes.Status500InternalServerError, new
				{
					message = "An error occurred while processing your request.",
					error = ex.Message
				});
			}
		}

		[HttpPost("refresh")]
		[AllowAnonymous]
		public async Task<ActionResult<AuthResponseDto>> Refresh([FromBody] RefreshTokenDto dto)
		{
			var result = await _sender.Send(new RefreshTokenCommand(dto.Token, dto.RefreshToken));
			return result is null ? Unauthorized(new { message = "Invalid or expired refresh token" }) : Ok(result);
		}
		//[HttpPost("refresh")]
		//public async Task<IActionResult> Refresh(TokenRequest request)
		//{
		//	var storedToken = await _context.RefreshTokens
		//		.FirstOrDefaultAsync(x => x.Token == request.RefreshToken);

		//	if (storedToken == null || storedToken.IsRevoked)
		//		return Unauthorized("Invalid refresh token");

		//	if (storedToken.ExpiryDate < DateTime.UtcNow)
		//		return Unauthorized("Expired refresh token");

		//	var user = await _userManager.FindByIdAsync(storedToken.UserId);

		//	if (user == null)
		//		return Unauthorized();

		//	// 🔁 Rotate refresh token
		//	storedToken.IsRevoked = true;

		//	var newRefreshToken = _tokenservice.GenerateRefreshToken();//   GenerateRefreshToken();

		//	var newRefreshTokenEntity = new RefreshToken
		//	{
		//		Token = newRefreshToken,
		//		UserId = user.Id,
		//		ExpiryDate = DateTime.UtcNow.AddDays(7),
		//		CreatedAt = DateTime.UtcNow
		//	};

		//	_context.RefreshTokens.Add(newRefreshTokenEntity);

		//	var newAccessToken = _tokenservice.GenerateToken(login.email); //GenerateJwtToken(user);

		//	await _context.SaveChangesAsync();

		//	return Ok(new
		//	{
		//		accessToken = newAccessToken,
		//		refreshToken = newRefreshToken
		//	});
		//}

	}
}
