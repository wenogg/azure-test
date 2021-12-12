using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace AzureTest.Api {
	public class Program {
		public static void Main(string[] args) {
			CreateHostBuilder(args).Build().Run();
		}

		public static IWebHostBuilder CreateHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration(ConfigConfiguration)
				.UseKestrel()
				.UseIISIntegration()
				.UseStartup<Startup>();


		static void ConfigConfiguration(WebHostBuilderContext ctx, IConfigurationBuilder config) {
			config.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{ctx.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);


			config
				.AddEnvironmentVariables();

			var root = config.Build();
			var generalSettingsPath = root.GetValue<string>("ConnectionStrings:DefaultConnection");
			
		}
	}
}
