
using Hub.Server.Interfaces;
using Hub.Server.Models;
using Hub.Shared.Model;
using Hub.Shared.Model.Hub;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NetworkLibrary;
using System.Linq.Expressions;

namespace Hub.Server.SignalR
{
    public class ServerTimeNotifier : BackgroundService
    {
        private static readonly TimeSpan period = TimeSpan.FromSeconds(5);
        private readonly ILogger<ServerTimeNotifier> _logger;
        private readonly IHubContext<NotificationHub, iNotifiCationClient> _hubContext;
        private readonly XpVoiceDbContext _voiceContext;

        public ServerTimeNotifier()
        {

        }

        public ServerTimeNotifier(ILogger<ServerTimeNotifier> logger, IHubContext<NotificationHub, iNotifiCationClient> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            using var timer = new PeriodicTimer(period);
            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                var datetime = DateTime.Now;
                SignalRMessage<string> testData = new SignalRMessage<string>
                {
                    type = Shared.MessageType.Alert,
                    title = "Server Time",
                    body = datetime.ToString("yyyy-MM-dd hh:mm:ss")
                };

                string message = testData.EncryptData();
                
                /*
                foreach ( var temp in NotificationHub.GetConnectedUsers())
                {
                    await _hubContext.Clients.Group(temp.groupName).ReceiveNotification(message);
                }*/                
            }
        }
    }
}
