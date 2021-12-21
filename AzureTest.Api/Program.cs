using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace AzureTest.Api {
	/// <summary>
	/// 
	/// </summary>
	public class Program {
		public static void Main(string[] args) {
			CreateHostBuilder(args).Build().Run();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public static IWebHostBuilder CreateHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration(ConfigConfiguration)
				.UseKestrel()
				.UseIISIntegration()
				.UseStartup<Startup>();


		/// <summary>
		/// 
		/// </summary>
		/// <param name="ctx"></param>
		/// <param name="config"></param>
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
