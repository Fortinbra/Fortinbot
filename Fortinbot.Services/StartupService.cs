using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Fortinbot.Services
{
	public class StartupService : IHostedService
	{
		private readonly IServiceProvider _provider;
		private readonly DiscordSocketClient _discord;
		private readonly CommandService _commands;
		public StartupService(
		  IServiceProvider provider,
		  DiscordSocketClient discord,
		  CommandService commands)
		{
			_provider = provider;
			_discord = discord;
			_commands = commands;
		}
		public async Task StartAsync()
		{
			string? discordToken = Environment.GetEnvironmentVariable("DiscordToken");
			if (string.IsNullOrWhiteSpace(discordToken))
				throw new Exception("Please enter your bot's token into the `_configuration.json` file found in the applications root directory.");

			await _discord.LoginAsync(TokenType.Bot, discordToken);
			await _discord.StartAsync();

			await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			string? discordToken = Environment.GetEnvironmentVariable("DiscordToken");
			if (string.IsNullOrWhiteSpace(discordToken))
				throw new Exception("Please enter your bot's token into the `_configuration.json` file found in the applications root directory.");

			await _discord.LoginAsync(TokenType.Bot, discordToken);
			await _discord.StartAsync();

			await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
			await _discord.SetStatusAsync(UserStatus.Online);
			await _discord.SetGameAsync("no one, you're not my real mom!", null, ActivityType.Listening);
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
