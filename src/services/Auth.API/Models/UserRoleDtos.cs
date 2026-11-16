namespace Auth.API.Models
{
	public class UserRoleDtos
	{
	}

	public record RoleDto(Guid Id, string Name, string? Description, DateTime CreatedAt);

	public record CreateRoleDto(string Name, string? Description);

	public record UpdateRoleDto(Guid Id, string Name, string? Description);

	public record UserDto(
		Guid Id,
		string FullName,
		string Email,
		Guid? RoleId,
		string? RoleName,
		bool IsActive,
		DateTime? LastLogin,
		DateTime CreatedAt
	);

	public record CreateUserDto(
		string FullName,
		string Email,
		string Password,
		Guid? RoleId,
		bool IsActive = true
	);

	public record UpdateUserDto(
		Guid Id,
		string FullName,
		string Email,
		Guid? RoleId,
		bool IsActive
	);

	public record LoginDto(string Email, string Password);

	public record AuthResponseDto(string Token, string RefreshToken, string FullName, string Email, string? Role, DateTime Expires);

	public record RefreshTokenDto(string Token, string RefreshToken);

	public record PagedResult<T>(List<T> Items, int TotalCount, int Page, int PageSize);

}
