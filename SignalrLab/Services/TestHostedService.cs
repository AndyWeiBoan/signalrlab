using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace SignalrLab.Services
{
    public class TestHostedService : IHostedService
    {
        private Channel<int> _channel = Channel.CreateUnbounded<int>();
        
        public TestHostedService(IHostApplicationLifetime life)
        {
            life.ApplicationStopping.Register(async () =>
            {
                Console.WriteLine("ZZZZZZZZZZZZZZZZZZ");
                await Task.Delay(10000);
                Console.WriteLine("AAAAAAAAAAAAAAAAAAA");
            });
        }
        Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            this.ConsumeEventAsync();

            return Task.CompletedTask;
        }

        async Task IHostedService.StopAsync(CancellationToken cancellationToken)
        {

        }

        async Task ConsumeEventAsync()
        {
            var reader = _channel.Reader;
            while (await _channel.Reader.WaitToReadAsync())
            {
                if (reader.TryRead(out var task))
                {
                    
                }
            }
        }
    }
}
