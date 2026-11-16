using Auth.API.Models;

namespace Auth.API.Interfaces
{
	public interface IUserRepository
	{
		Task<PagedResult<UserDto>> GetAllAsync(int page, int pageSize, string? search, CancellationToken ct = default);
		Task<UserDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
		Task<UserDto> CreateAsync(string fullName, string email, string password, Guid? roleId, bool isActive, CancellationToken ct = default);
		Task<UserDto?> UpdateAsync(Guid id, string fullName, string email, Guid? roleId, bool isActive, CancellationToken ct = default);
		Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
		Task<AuthResponseDto?> LoginAsync(string email, string password, CancellationToken ct = default);
		Task<AuthResponseDto?> RefreshTokenAsync(string token, string refreshToken, CancellationToken ct = default);
	}

	public interface IRoleRepository
	{
		Task<List<RoleDto>> GetAllAsync(CancellationToken ct = default);
		Task<RoleDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
		Task<RoleDto> CreateAsync(string name, string? description, CancellationToken ct = default);
		Task<RoleDto?> UpdateAsync(Guid id, string name, string? description, CancellationToken ct = default);
		Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
	}

}
