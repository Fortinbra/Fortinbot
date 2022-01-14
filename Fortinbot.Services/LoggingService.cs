using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortinbot.Services
{
	public class LoggingService
	{
		private readonly DiscordSocketClient _discordSocketClient;
		private readonly CommandService _commandService;

		private string _logDirectory { get; }
		private string _logFile => Path.Combine(_logDirectory, $"{DateTime.UtcNow.ToString("dd-MM-yyyy")}.txt");

		public LoggingService(DiscordSocketClient discordSocketClient, CommandService commandService)
		{
			_logDirectory = Path.Combine(AppContext.BaseDirectory, "logs");
			_discordSocketClient = discordSocketClient ?? throw new ArgumentNullException(nameof(discordSocketClient));
			_commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

			_discordSocketClient.Log += OnLogAsync;
			_commandService.Log += OnLogAsync;
		}

		private Task OnLogAsync(LogMessage message)
		{
			if (!Directory.Exists(_logDirectory))
				Directory.CreateDirectory(_logDirectory);
			if (!File.Exists(_logFile))
				File.Create(_logFile).Dispose();

			string logText = $"{DateTime.UtcNow.ToString("hh:mm:ss")} [{message.Severity}] {message.Source}: {message.Exception?.ToString() ?? message.Message}";
			File.AppendAllText(_logFile, logText + "\n");

			return Console.Out.WriteLineAsync(logText);
		}
	}
}
