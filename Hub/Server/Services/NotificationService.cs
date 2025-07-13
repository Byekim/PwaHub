using Hub.Shared.Model.Hub.Login;
using Hub.Shared.Model.Hub;
using Microsoft.EntityFrameworkCore;
using Hub.Server.Interfaces.Service;
using System.Collections.Concurrent;
using Hub.Server.Interfaces;
using Hub.Server.SignalR;
using Microsoft.AspNetCore.SignalR;
using Hub.Server.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Hub.Client.Pages;

namespace Hub.Server.Services
{
    public class NotificationService : iNotificationService
    {
        private readonly IHubContext<NotificationHub, iNotifiCationClient> _hubContext;
        private static readonly ConcurrentDictionary<string, ConncectedUser> connectedUsers = new();
        public NotificationService(IHubContext<NotificationHub, iNotifiCationClient> hubContext)
        {            
            _hubContext = hubContext;
        }

        #region Message 
        public async Task SendMessage(string clientId, string data)
        {
            await _hubContext.Clients.Client(clientId).ReceiveNotification(data);
        }

        public async Task BroadcastMessage(string data)
        {
            await _hubContext.Clients.All.ReceiveNotification(data);
        }
        #endregion

        #region Group
        public async Task JoinGroup(string code, string connectionId)
        {
            var user = connectedUsers.Values.FirstOrDefault(u => u.connectionId == connectionId);
            if (user != null)
            {
                user.groupName = code;
                await _hubContext.Groups.AddToGroupAsync(connectionId, code);
            }
        }

        public async Task LeaveGroup(string code, string connectionId)
        {
            // 그룹에서 사용자를 제거
            var user = connectedUsers.Values.FirstOrDefault(u => u.connectionId == connectionId);
            if (user != null && user.groupName == code)
            {
                user.groupName = string.Empty;
                await _hubContext.Groups.RemoveFromGroupAsync(connectionId, code);
            }
        }

        public async Task<List<ConncectedUser>> GetUsersInGroup(string code)
        {
            return  connectedUsers.Values.Where(u => u.xpErpUserData.aptCd == code ).ToList();
        }


        public List<ConncectedUser> FindUsersInGroup(string code)
        {
            return connectedUsers.Values.Where(u => u.groupName == code).ToList();
        }
        #endregion

        #region 유저관리
        
        public Task Login(string connectionId, ResponseXperpLogin login)
        {
            var user = connectedUsers.TryGetValue(login.userId, out var existingUser) ? existingUser : null;

            if (user != null)
            {
                // 사용자가 이미 접속해 있는 경우
                user.connectionId = connectionId; // 최신 connectionId로 업데이트
                user.connctedTime = DateTime.Now; // 접속 시간 갱신
                user.xpErpUserData = login; // 사용자 정보 업데이트
            }
            else
            {
                // 새로운 사용자 접속
                var newUser = new ConncectedUser
                {
                    connectionId = connectionId,
                    connctedTime = DateTime.Now,
                    xpErpUserData = login // 사용자 정보 초기화
                };
                connectedUsers.TryAdd(login.userId, newUser);
            }

            return Task.CompletedTask;
        }

        public async Task LogOut(string connectionId)
        {
            if (connectedUsers.TryRemove(connectionId, out var user))
            {                
                await _hubContext.Clients.Client(user.connectionId).SessionEnded("SessionEnded");
            }
            else
            {
                // 로그 기록
            }
        }

        public async Task<List<ConncectedUser>> GetConncectedUsers(string code = "")
        {
             return connectedUsers.Values.Where(u => u.groupName == code).ToList();
        }
        #endregion
    }
}
