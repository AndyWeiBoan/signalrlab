using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
namespace Client {
    class Program {

        private static Channel<int> _channel = Channel.CreateUnbounded<int>();

        public static async Task SingleProducerSingleConsumer()
        {
            for (int i = 0; i < 10; i++)
            {
                await _channel.Writer.WriteAsync(i + 1);
            }
        }

        public static async Task ConsumeAsync()
        {
            var reader = _channel.Reader;
            while (await reader.WaitToReadAsync())
            {
                if (reader.TryRead(out var number))
                {
                    Console.WriteLine(number);
                }
            }
        }
        static async Task Main(string[] args) {
            //_ = ConsumeAsync();
            //await SingleProducerSingleConsumer();
            //Console.Read();
            try
            {
                await Task.Delay(5000);
                var jwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJqanRlc3QxMCIsImp0aSI6IjM1YjgxODdiLWIzYzQtNDkxMi1hOGQ1LTdiOTFkZWJlNTUwZiIsIm1lcmNoYW50Q29kZSI6Ijg3Njg4IiwicGxheWVySWQiOiJqanRlc3QxMCIsInRlbmFudFR5cGUiOiJQbGF5ZXIiLCJza2lsbElkIjoiZjBjMmQ1OGEtODM0Yy00NWI2LTkyNGMtMjBhNWRkOTQ3YTVkIiwicm9vbUlkIjoiMGRhNDU5NWQtNmZhOS00OGMyLTA5MzItMDhkOTc5YjYzMDM5IiwibmJmIjoxNjMxOTUyNjM5LCJleHAiOjE2MzIwMzkwMzksImlhdCI6MTYzMTk1MjYzOSwiaXNzIjoiSGlnZ3MuQ2hhdHRpbmcuQXBpIn0.IQ2svLbMAuVdHpSiVGH5tIaocaLp4kouh0g5gvHRviA";
                var connection = new HubConnectionBuilder()
                .WithUrl($"http://localhost:5000/chatHub?access_token={jwt}", options =>
                {
                    //options.SkipNegotiation = true;
                    //options.Transports = HttpTransportType.WebSockets;
                    //options.UseDefaultCredentials = true;
                    options.AccessTokenProvider = () => Task.FromResult(jwt);
                })
                .Build();



                //connection.On<string>("Send", data =>
                //{
                //    Console.WriteLine($"Received: {data}");
                //});
                connection.Closed += (ex) =>
                {
                    Console.WriteLine($"disconnect : {ex?.ToString()}");

                    return Task.CompletedTask;
                };

                connection.On<string>("SendMessage", (message) =>
                {
                    Console.WriteLine($"Received Message: {message}");
                });

                connection.On<string, string>("SendMessage", (user, message) =>
                {
                    Console.WriteLine(message);
                });
                await connection.StartAsync();

                while(true)
                {
                    await connection.InvokeAsync("SendMessage", "test");
                }

                Console.Read();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
                ;
            }





        }
    }
}
