using System.Collections.Concurrent;

using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.SignalR;

namespace SignalrLab.Services {
    public interface IHubCallerManager {

        void Watch(HubCallerContext context);

        void Abort(string connectionId);
    }

    public class HubCallerManager : IHubCallerManager {

        private readonly ConcurrentDictionary<string, string> _callsers;
        private readonly ConcurrentDictionary<string, string> _dismisses;

        public HubCallerManager() {
            _callsers = new ConcurrentDictionary<string, string>();
            _dismisses = new ConcurrentDictionary<string, string>();
        }

        void IHubCallerManager.Abort(string connectionId) {
            _dismisses.TryAdd(connectionId, "");
        }

        void IHubCallerManager.Watch(HubCallerContext context) {
            var feature = context.Features.Get<IConnectionHeartbeatFeature>();
            _callsers.TryAdd("", "");
            feature.OnHeartbeat(state => {
                if (_dismisses.TryGetValue(context.ConnectionId, out var value)) {
                    context.Abort();
                    _callsers.TryRemove(context.ConnectionId, out var o);
                }

            }, context.ConnectionId);
        }
    }
}
