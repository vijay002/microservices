using Auth.API.Models;

namespace Auth.API.Extension
{
    public static class IServiceCollectionExtensions
    {
		public static void AddConsumptionLayer(this IServiceCollection services, IConfiguration configuration)
		{
			//services.AddMappings();
			//services.AddDbContext(configuration);
			//services.AddRepositories();
			//services.AddScoped<SignInManager<ApplicationUser>, ProjSignInManager<ApplicationUser>>();
			//services.AddScoped<RabbitMqConsumer>();
		}
	}
}
