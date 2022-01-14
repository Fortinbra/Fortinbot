using Discord;
using Discord.WebSocket;
using Fortinbot.Handlers;
using Fortinbot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Fortinbot
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Program Entry point")]
	internal class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
			.ConfigureServices((hostcontext, services) =>
			{
				services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
				{
					LogLevel = LogSeverity.Verbose,
					MessageCacheSize = 2048,
				}));
				services.AddSingleton<CommandHandler>();
				services.AddSingleton<StartupService>();
				services.AddSingleton<LoggingService>();
				services.AddSingleton<Random>();

				services.AddHostedService<CountdownService>();

			});
	}
}
