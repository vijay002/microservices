using System.Data;
using Application.Common.Repositories;
using Domain.Entities;
using Infrastructure.DataAccessManager.EFCore.Configurations;
using Infrastructure.SecurityManager.AspNetIdentity;
using Infrastructure.SeedManager.Systems;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.DataAccessManager.EFCore.Contexts;


public class DataContext : IdentityDbContext<ApplicationUser>, IEntityDbSet
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
       // base.Database.SetConnectionString("Server=DESKTOP-T2D5842;Database=mymicroservice;User=sa;Password=sa123;TrustServerCertificate=True;");
    }

	public DbSet<Token> Token { get; set; }
	public DbSet<Company> Company { get; set; }
    public DbSet<FileImage> FileImage { get; set; }
    public DbSet<FileDocument> FileDocument { get; set; }


    public DbSet<NumberSequence> NumberSequence { get; set; }
    public DbSet<CustomerGroup> CustomerGroup { get; set; }
    public DbSet<CustomerCategory> CustomerCategory { get; set; }
   
    public DbSet<Customer> Customer { get; set; }
   
    public DbSet<CustomerContact> CustomerContact { get; set; }

	public DbSet<VendorGroup> VendorGroup { get; set; }
	public DbSet<VendorCategory> VendorCategory { get; set; }
	public DbSet<Vendor> Vendor { get; set; }
	public DbSet<UnitMeasure> UnitMeasure { get; set; }
	public DbSet<ProductGroup> ProductGroup { get; set; }
	
	public DbSet<Product> Product { get; set; }
	//public DbSet<CustomerContact> CustomerContact { get; set; }
	public DbSet<VendorContact> VendorContact { get; set; }
	public DbSet<Tax> Tax { get; set; }
	public DbSet<SalesOrder> SalesOrder { get; set; }
	public DbSet<SalesOrderItem> SalesOrderItem { get; set; }
	public DbSet<PurchaseOrder> PurchaseOrder { get; set; }
	public DbSet<PurchaseOrderItem> PurchaseOrderItem { get; set; }
	public DbSet<SalesTeam> SalesTeam { get; set; }
	public DbSet<SalesRepresentative> SalesRepresentative { get; set; }
	protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new FileImageConfiguration());
        modelBuilder.ApplyConfiguration(new FileDocumentConfiguration());


        modelBuilder.ApplyConfiguration(new NumberSequenceConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerGroupConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerContactConfiguration());

		//modelBuilder.ApplyConfiguration(new VendorGroupConfiguration());
		//modelBuilder.ApplyConfiguration(new VendorCategoryConfiguration());
		modelBuilder.ApplyConfiguration(new CustomerConfiguration());
		//modelBuilder.ApplyConfiguration(new VendorConfiguration());
		//modelBuilder.ApplyConfiguration(new UnitMeasureConfiguration());
		//modelBuilder.ApplyConfiguration(new ProductGroupConfiguration());
		//modelBuilder.ApplyConfiguration(new ProductConfiguration());
		modelBuilder.ApplyConfiguration(new CustomerContactConfiguration());
		//modelBuilder.ApplyConfiguration(new VendorContactConfiguration());
		//modelBuilder.ApplyConfiguration(new TaxConfiguration());
		//modelBuilder.ApplyConfiguration(new SalesOrderConfiguration());
		//modelBuilder.ApplyConfiguration(new SalesOrderItemConfiguration());
		//modelBuilder.ApplyConfiguration(new PurchaseOrderConfiguration());
		//modelBuilder.ApplyConfiguration(new PurchaseOrderItemConfiguration());
	}

}


public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
	public DataContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

		optionsBuilder.UseSqlServer("Server=DESKTOP-T2D5842;Database=mymicroservice;User Id=sa;Password=sa123;TrustServerCertificate=True;MultipleActiveResultSets=True;");

		//using var scope = host.Services.CreateScope();
		//var serviceProvider = scope.ServiceProvider;

		//var context = serviceProvider.GetRequiredService<DataContext>();
		//if (!context.Roles.Any()) //if empty, thats mean never been seeded before
		//{
		//	var roleSeeder = serviceProvider.GetRequiredService<RoleSeeder>();
		//	roleSeeder.GenerateDataAsync().Wait();

		//	var userAdminSeeder = serviceProvider.GetRequiredService<UserAdminSeeder>();
		//	userAdminSeeder.GenerateDataAsync().Wait();

		//	var companySeeder = serviceProvider.GetRequiredService<CompanySeeder>();
		//	companySeeder.GenerateDataAsync().Wait();

		//}



		return new DataContext(optionsBuilder.Options);
	}
}


