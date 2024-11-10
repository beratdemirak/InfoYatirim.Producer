namespace InfoYatirim.Producer
{
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class DataPublisherBackgroundService : BackgroundService
    {
        private readonly PublisherService _publisherService;
        private static readonly Random _random = new();

        public DataPublisherBackgroundService(PublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _publisherService.CreateQueueAsync();
            while (!stoppingToken.IsCancellationRequested)
            {
                var Data = new Data
                {
                    O = _random.Next(1, 1000),
                    H = _random.Next(1, 1000),
                    L = _random.Next(1, 1000),
                    C = _random.Next(1, 1000),
                    V = (decimal)_random.NextDouble() * 100,
                    T = DateTime.Now
                };

                await _publisherService.PublishDataAsync(Data);

                await Task.Delay(1000, stoppingToken);
            }
        }

        public override void Dispose()
        {
            _publisherService.Dispose();
            base.Dispose();
        }
    }
}
