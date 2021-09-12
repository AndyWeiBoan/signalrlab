using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
namespace Client {
    class Program {
        static async Task Main(string[] args) {
            try {
                await Task.Delay(5000);
                var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/chatHub")
                .Build();

                //connection.On<string>("Send", data =>
                //{
                //    Console.WriteLine($"Received: {data}");
                //});
                connection.Closed +=  (ex) => {
                    Console.WriteLine($"disconnect : {ex?.ToString()}");

                    return Task.CompletedTask;
                };
                await connection.StartAsync();

                while (true) {
                    await connection.InvokeAsync("SendMessage", "2131231231");
                }
            } catch (Exception ex) {

                Console.WriteLine(ex.ToString());
                ;
            }
            

            
           
            
        }
    }
}
