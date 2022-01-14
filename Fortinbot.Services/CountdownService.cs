using Cronos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Fortinbot.Services
{
	public class CountdownService : IHostedService, IDisposable
	{
		private readonly ILogger _logger;
		private System.Timers.Timer _timer;
		private readonly CronExpression _cronExpression;
		public CountdownService(ILogger<CountdownService> logger, IHostApplicationLifetime appLifetime)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_cronExpression = CronExpression.Parse("*/1 * * * *");
			//appLifetime.ApplicationStarted.Register(OnStarted);
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

		public virtual async Task StartAsync(CancellationToken cancellationToken)
		{
			await ScheduledJob(cancellationToken);
		}

		private async Task ScheduledJob(CancellationToken cancellationToken)
		{
			var next = _cronExpression.GetNextOccurrence(DateTime.UtcNow);
			if (next.HasValue)
			{
				var delay = next.Value - DateTimeOffset.Now;
				if (delay.TotalMilliseconds <= 0)
				{
					await ScheduledJob(cancellationToken);
				}
				_timer = new System.Timers.Timer(delay.TotalMilliseconds);
				_timer.Elapsed += async (sender, args) =>
				 {
					 _timer.Dispose();
					 _timer = null;
					 if (!cancellationToken.IsCancellationRequested)
					 {
						 Console.WriteLine(DateTime.Now.ToString());
						 await ScheduledJob(cancellationToken);
					 }

				 };
				_timer.Start();

			}
			await Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}