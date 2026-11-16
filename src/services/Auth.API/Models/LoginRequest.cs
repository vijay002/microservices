namespace Auth.API.Models;

public class LoginRequest
{
	//public required  string Username { get; set; }
	public required string email { get; set; }
	public required string password { get; set; }
}
