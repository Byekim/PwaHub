using Hub.Server.Interfaces;
using Hub.Server.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace Hub.Server.SignalR
{
    public class SessionTimeoutService : BackgroundService
    {
        private readonly IHubContext<NotificationHub, iNotifiCationClient> _hubContext;

        public SessionTimeoutService(IHubContext<NotificationHub, iNotifiCationClient> hubContext)
        {
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await NotificationHub.CheckSessionTimeouts(_hubContext);
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // 1분마다 확인
            }
        }
    }
}
    