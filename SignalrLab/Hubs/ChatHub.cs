using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalrLab.Services;

namespace SignalrLab.Hubs {

    public class PlayerIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.GetHttpContext().Request.Query["access_token"];
        }
    }

    public interface IChatHub {
        // 這個方法是用來發出 Message 給 Client
        Task SendMessage(string message);
    }

    [Authorize]
    public class ChatHub : Hub<IChatHub> {
        public Task SendMessage(string data) {           

            return Clients.All.SendMessage($"{data}");
        }

        public override Task OnConnectedAsync() {
            var h = Context.GetHttpContext().Request.Headers.ToList();
            HubCallerContext c = base.Context;
            Groups.AddToGroupAsync(c.ConnectionId, "A");
            Clients.Client("21312").SendMessage("sdfdsfs");
            Clients.Client(base.Context.ConnectionId).SendMessage("sdfdsfs");
            Groups.RemoveFromGroupAsync("sdfds", "dsfdsfsd");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception) {
            var g = Groups;
            return base.OnDisconnectedAsync(exception);
        }
    }
}
