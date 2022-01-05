using Discord;
using Discord.WebSocket;
using Fortinbot.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortinbot
{
	internal class Program
	{
		private static DiscordSocketClient _client;

		//public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
			.ConfigureServices((hostcontext, services) =>
			{
				services.AddScoped<DiscordSocketClient>(async _ =>
				{
					_client = new DiscordSocketClient();

					await _client.LoginAsync(TokenType.Bot,
					Environment.GetEnvironmentVariable("DiscordToken"));
					await _client.StartAsync();
					return _client;
				});
				services.AddHostedService<CountdownService>();
			});

		public async Task MainAsync()
		{
			_client = new DiscordSocketClient();
			_client.Log += Log;
			await _client.LoginAsync(TokenType.Bot,
				Environment.GetEnvironmentVariable("DiscordToken"));
			await _client.StartAsync();

			// Block this task until the program is closed.
			await Task.Delay(-1);
		}
		private Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}
	}
}
