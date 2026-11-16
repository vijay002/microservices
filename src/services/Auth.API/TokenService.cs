using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Microsoft.IdentityModel.Tokens;

namespace Auth.API
{
	/// <summary>
	/// xxxxx – Header,  yyyyy – Payload, zzzzz – Signature
	/// </summary>
	public class TokenService
	{
		private readonly IConfiguration _config;

		public TokenService(IConfiguration configuration)
		{
			_config = configuration;
		}

		public string GenerateToken(string username)
		{
			var jwt = _config.GetSection("Jwt");
			if (jwt != null && jwt["Key"] != null && !string.IsNullOrEmpty(jwt["Key"]))
			{
				var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"] ?? ""));
				var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
				var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, username),
						new Claim(ClaimTypes.Role, "User")
					};

				var token = new JwtSecurityToken(
				 issuer: jwt["Issuer"],
				 audience: jwt["Audience"],
				 claims: claims,
				 expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwt["ExpireMinutes"])),
				 signingCredentials: creds
					);
				if (token != null)
					return new JwtSecurityTokenHandler().WriteToken(token);
				else
					return new JwtSecurityTokenHandler().WriteToken(null);
			}
			return new JwtSecurityTokenHandler().WriteToken(null);
		}

		//private async Task<string> GenerateRefreshTokenAsync(string token, User user, string? existingRefreshToken)
		//{
		//	var tokenHandler = new JwtSecurityTokenHandler();
		//	var jwtToken = tokenHandler.ReadJwtToken(token);
		//	var jti = jwtToken.Id;

		//	var refreshToken = new RefreshToken
		//	{
		//		Token = Guid.NewGuid().ToString(),
		//		JwtId = jti,
		//		UserId = user.Id,
		//		ExpiryDate = DateTime.UtcNow.AddDays(7),
		//		CreatedAtUtc = DateTime.UtcNow,
		//	};

		//	if (!string.IsNullOrEmpty(existingRefreshToken))
		//	{
		//		var existingToken = await _dbContext.RefreshTokens
		//			.FirstOrDefaultAsync(x => x.Token == existingRefreshToken);

		//		if (existingToken != null)
		//		{
		//			_dbContext.Set<RefreshToken>().Remove(existingToken);
		//		}
		//	}

		//	await _dbContext.AddAsync(refreshToken);
		//	await _dbContext.SaveChangesAsync();

		//	return refreshToken.Token;
		//}

		public string GenerateRefreshToken()
		{
			var randomBytes = new byte[64];
			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomBytes);
			return Convert.ToBase64String(randomBytes);
		}
	}
}
