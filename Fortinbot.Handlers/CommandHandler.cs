using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace Fortinbot.Handlers
{
	public class CommandHandler
	{
		private readonly DiscordSocketClient _discordSocketClient;
		private readonly CommandService _commandService;
		private readonly IConfigurationRoot _config;
		private readonly IServiceProvider _serviceProvider;

		public CommandHandler(DiscordSocketClient discordSocketClient, CommandService commandService, IConfigurationRoot config, IServiceProvider serviceProvider)
		{
			_discordSocketClient = discordSocketClient ?? throw new ArgumentNullException(nameof(discordSocketClient));
			_commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
			_config = config ?? throw new ArgumentNullException(nameof(config));
			_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

			_discordSocketClient.MessageReceived += OnMessageRecievedAsync;
		}

		private async Task OnMessageRecievedAsync(SocketMessage message)
		{
			var msg = message as SocketUserMessage;
			if (msg == null) return;
			if (msg.Author.Id == _discordSocketClient.CurrentUser.Id) return;
			var context = new SocketCommandContext(_discordSocketClient, msg);

			int argPos = 0;
			if (msg.HasStringPrefix(_config["prefix"], ref argPos) || msg.HasMentionPrefix(_discordSocketClient.CurrentUser, ref argPos))
			{
				var result = await _commandService.ExecuteAsync(context, argPos, _serviceProvider);

				if (!result.IsSuccess)
				{
					await context.Channel.SendMessageAsync(result.ToString());
				}
			}
		}
	}
}
