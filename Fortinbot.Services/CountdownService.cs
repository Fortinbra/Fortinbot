using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Fortinbot.Services
{
	public class CountdownService : IHostedService
	{
		private readonly ILogger _logger;
		public CountdownService(ILogger<CountdownService> logger, IHostApplicationLifetime appLifetime)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			appLifetime.ApplicationStarted.Register(OnStarted);
			appLifetime.ApplicationStopping.Register(OnStopping);
			appLifetime.ApplicationStopped.Register(OnStopped);
		}

		private void OnStopped()
		{
			throw new NotImplementedException();
		}

		private void OnStopping()
		{
			throw new NotImplementedException();
		}

		private void OnStarted()
		{
			throw new NotImplementedException();
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}