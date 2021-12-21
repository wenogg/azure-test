using AzureTest.Core.Entities;
using AzureTest.Core.Idenity;
using AzureTest.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost
	.UseKestrel()
	.UseIISIntegration();

// Add services to the container.

// Data Contexts
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SandboxDBContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


// Authentication
builder.Services
	.AddIdentity<ApplicationUser, ApplicationRole>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddDefaultUI()
	.AddDefaultTokenProviders()
	.AddEntityFrameworkStores<SandboxDBContext>();

builder.Services.AddAuthentication(options => {
	options.DefaultScheme = IdentityConstants.ApplicationScheme;	
})
.AddCookie()
.AddGoogle(options => {
	options.SignInScheme = IdentityConstants.ExternalScheme;
	options.ClientId = builder.Configuration["Google:ClientId"];
	options.ClientSecret = builder.Configuration["Google:ClientSecret"];

});

builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();