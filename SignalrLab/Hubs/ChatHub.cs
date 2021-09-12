using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalrLab.Services;

namespace SignalrLab.Hubs {
    public interface IChatHub {
        // 這個方法是用來發出 Message 給 Client
        Task SendMessage(string message);
    }
    public class ChatHub : Hub<IChatHub> {
        public Task SendMessage(string data) {           

            return Clients.All.SendMessage($"{data}");
        }

        public override Task OnConnectedAsync() {
            var h = Context.GetHttpContext().Request.Headers.ToList();
            HubCallerContext c = base.Context;
            Groups.AddToGroupAsync(c.ConnectionId, "A");
            Groups.AddToGroupAsync(c.ConnectionId, "B");
            Groups.AddToGroupAsync(c.ConnectionId, "C");
            Groups.AddToGroupAsync(c.ConnectionId, "D");
            Groups.AddToGroupAsync(c.ConnectionId, "E");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception) {
            var g = Groups;
            return base.OnDisconnectedAsync(exception);
        }
    }
}
