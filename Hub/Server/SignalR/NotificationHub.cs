using Hub.Server.Interfaces;
using Hub.Server.Interfaces.Service;
using Hub.Server.Interfaces.Service.Voice;
using Hub.Server.Models;
using Hub.Server.Services;
using Hub.Shared;
using Hub.Shared.Model;
using Hub.Shared.Model.Hub;
using Hub.Shared.Model.Hub.Login;
using Hub.Shared.Voice;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetworkLibrary;
using StackExchange.Redis;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Hub.Server.SignalR
{
    public class NotificationHub : Hub<iNotifiCationClient>
    {
        iNotificationService _notificationService;
        iVoiceBroadcastService _voiceBroadcastService;

        public NotificationHub(iNotificationService notificationService, iVoiceBroadcastService voiceBroadcastService )
        {
            _notificationService = notificationService;
            _voiceBroadcastService = voiceBroadcastService;
        }

        #region 유저관리
        public override async Task OnConnectedAsync()
        {
            var token = Context.GetHttpContext()?.Request.Query["access_token"];
            if(await ValidateTokenAsync(token) == false)
            {

                Console.WriteLine("Invalid token detected, disconnecting user.");
                Context.Abort();
                return;
            }
            await base.OnConnectedAsync();
        }



        public async Task Login(ResponseXperpLogin loginData)
        {
            await _notificationService.Login(Context.ConnectionId, loginData);
        }

        public async Task LogOut(ResponseXperpLogin loginData)
        {
            await _notificationService.LogOut(Context.ConnectionId);
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (exception != null)
            {
                Console.WriteLine($"Disconnected with exception: {exception.Message}");
            }
            await base.OnDisconnectedAsync(exception);
        }

        #endregion

        #region 그룹관리
        public async Task JoinGroupAsync(string groupName)
        {
            await _notificationService.JoinGroup(Context.ConnectionId, groupName);
        }

        public async Task LeaveGroup(string connectionId, string code)
        {
            await _notificationService.LeaveGroup(connectionId, code);
        }

        public async Task<List<ConncectedUser>> GetUsersInGroup(string code)
        {
            return await _notificationService.GetUsersInGroup(code);
        }
        #endregion

        #region Message처리
        public async Task SendMessage( string data)
        {
            await _notificationService.SendMessage(Context.ConnectionId, data);
        }
        
        public async Task BroadcastMessage(string data)
        {
            await _notificationService.BroadcastMessage(data);
        }

        #endregion

        #region Voice
        public async Task<ResultMsgStatus> AddVoiceBroadcast(VoiceBroadCast voiceBroadcast)
        {
            return await _voiceBroadcastService.AddVoiceBroadcast(voiceBroadcast);
        }

        public async Task<ResultMsgStatus> UpdateVoiceBroadcast(VoiceBroadCast voiceBroadcast)
        {
            return await _voiceBroadcastService.UpdateVoiceBroadcast(voiceBroadcast);
        }

        public async Task<ResultMsgStatus> DeletevoiceBroadcast(VoiceBroadCast voiceBroadcast)
        {
            return await _voiceBroadcastService.DeletevoiceBroadcast(voiceBroadcast);
        }

        public async Task<VoiceBroadCast> GetVoiceBroadcast(int seq, string aptCd)
        {
            return await _voiceBroadcastService.GetVoiceBroadcast(seq, aptCd);
        }

        public async Task<List<VoiceBroadCast>> GetAllVoiceBroadcast(string aptCd)
        {
            return await _voiceBroadcastService.GetAllVoiceBroadcast(aptCd);
        }

        public async Task<ResultMsgStatus> SetVoiceGroup(GroupMaster groupMaster)
        {
            return await _voiceBroadcastService.SetVoiceGroup(groupMaster);
        }

        public async Task<List<GroupMaster>> GetVoiceGroup(string aptCd)
        {
            return await _voiceBroadcastService.GetVoiceGroup(aptCd);
        }
        #endregion





        private async Task<bool?> ValidateTokenAsync(string? token)
        {
            if (string.IsNullOrEmpty(token)) 
                return null;

            try
            {
                // 토큰 형식 확인 (JWT)
                var handler = new JwtSecurityTokenHandler();
                if (!handler.CanReadToken(token))
                {
                    return false; // 잘못된 JWT 형식
                }

                var jwtToken = handler.ReadJwtToken(token);

                // 토큰 만료일 검사
                if (jwtToken.ValidTo < DateTime.UtcNow)
                    return false;

                return true;
            }
            catch (ArgumentException ex)
            {
                // 잘못된 토큰 형식
                Console.WriteLine($"Invalid token format: {ex.Message}");
                return false;
            }
            catch (SecurityTokenException ex)
            {
                // JWT 검증 중 오류 발생
                Console.WriteLine($"JWT verification error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // 기타 예외 처리
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return false;
            }

        }

        public static async Task CheckSessionTimeouts(IHubContext<NotificationHub, iNotifiCationClient> hubContext)
        {
            var now = DateTime.UtcNow;
            /*
            foreach (var user in connectedUsers.Values)
            {
                if (now - user.connctedTime > TimeSpan.FromHours(1))
                {
                    // 1시간 이상 활동 없음 -> 연결 끊기
                    await hubContext.Clients.Client(user.connectionId).SessionEnded("Your session has ended.");
                }
            }*/
        }

    }


}


