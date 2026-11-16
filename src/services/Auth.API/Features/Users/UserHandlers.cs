using Auth.API.Models;
using MediatR;

namespace Auth.API.Features.Users
{
	public class UserHandlers
	{
	}

	

	// ── Queries ──────────────────────────────────────────────────
	public record GetAllUsersQuery(int Page = 1, int PageSize = 20, string? Search = null)
		: IRequest<PagedResult<UserDto>>;

	//public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, PagedResult<UserDto>>
	//{
	//	private readonly IUserRepository _repo;
	//	public GetAllUsersHandler(IUserRepository repo) => _repo = repo;
	//	public Task<PagedResult<UserDto>> Handle(GetAllUsersQuery q, CancellationToken ct)
	//		=> _repo.GetAllAsync(q.Page, q.PageSize, q.Search, ct);
	//}

	public record GetUserByIdQuery(Guid Id) : IRequest<UserDto?>;

	//public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
	//{
	//	private readonly IUserRepository _repo;
	//	public GetUserByIdHandler(IUserRepository repo) => _repo = repo;
	//	public Task<UserDto?> Handle(GetUserByIdQuery q, CancellationToken ct) => _repo.GetByIdAsync(q.Id, ct);
	//}

	// ── Commands ─────────────────────────────────────────────────
	public record CreateUserCommand(string FullName, string Email, string Password, Guid? RoleId, bool IsActive)
		: IRequest<UserDto>;

	//public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserDto>
	//{
	//	private readonly IUserRepository _repo;
	//	public CreateUserHandler(IUserRepository repo) => _repo = repo;
	//	public Task<UserDto> Handle(CreateUserCommand cmd, CancellationToken ct)
	//		=> _repo.CreateAsync(cmd.FullName, cmd.Email, cmd.Password, cmd.RoleId, cmd.IsActive, ct);
	//}

	public record UpdateUserCommand(Guid Id, string FullName, string Email, Guid? RoleId, bool IsActive)
		: IRequest<UserDto?>;

	//public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UserDto?>
	//{
	//	private readonly IUserRepository _repo;
	//	public UpdateUserHandler(IUserRepository repo) => _repo = repo;
	//	public Task<UserDto?> Handle(UpdateUserCommand cmd, CancellationToken ct)
	//		=> _repo.UpdateAsync(cmd.Id, cmd.FullName, cmd.Email, cmd.RoleId, cmd.IsActive, ct);
	//}

	public record DeleteUserCommand(Guid Id) : IRequest<bool>;

	//public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
	//{
	//	private readonly IUserRepository _repo;
	//	public DeleteUserHandler(IUserRepository repo) => _repo = repo;
	//	public Task<bool> Handle(DeleteUserCommand cmd, CancellationToken ct) => _repo.DeleteAsync(cmd.Id, ct);
	//}

	// ── Auth ─────────────────────────────────────────────────────
	public record LoginCommand(string Email, string Password) : IRequest<AuthResponseDto?>;

	//public class LoginHandler : IRequestHandler<LoginCommand, AuthResponseDto?>
	//{
	//	//private readonly IUserRepository _repo;
	//	//public LoginHandler(IUserRepository repo) => _repo = repo;
	//	//public Task<AuthResponseDto?> Handle(LoginCommand cmd, CancellationToken ct)
	//	//	=> _repo.LoginAsync(cmd.Email, cmd.Password, ct);
	//}

	public record RefreshTokenCommand(string Token, string RefreshToken) : IRequest<AuthResponseDto?>;

	//public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDto?>
	//{
	//	private readonly IUserRepository _repo;
	//	public RefreshTokenHandler(IUserRepository repo) => _repo = repo;
	//	public Task<AuthResponseDto?> Handle(RefreshTokenCommand cmd, CancellationToken ct)
	//		=> _repo.RefreshTokenAsync(cmd.Token, cmd.RefreshToken, ct);
	//}


}
