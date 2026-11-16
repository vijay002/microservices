namespace Auth.API.Models
{
    public class RefreshToken
	{
		public int Id { get; set; }
		public string Token { get; set; } =string.Empty;
		public string UserId { get; set; } = string.Empty;	
		public DateTime ExpiryDate { get; set; }
		public bool IsRevoked { get; set; }
		public DateTime CreatedAt { get; set; }
	}


	public class TokenRequest
	{
		public string RefreshToken { get; set; } = string.Empty;
	}

}
