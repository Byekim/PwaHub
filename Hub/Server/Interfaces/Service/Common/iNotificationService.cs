using Hub.Shared.Model.Hub.Login;
using Hub.Shared.Model.Hub;

namespace Hub.Server.Interfaces.Service
{
    public interface iNotificationService
    {
        Task SendMessage(string clientId, string data);
        Task BroadcastMessage(string data);
        Task JoinGroup(string connectionId, string code);
        Task LeaveGroup(string connectionId, string code);
        Task<List<ConncectedUser>> GetUsersInGroup(string code);
        Task Login(string connectionId, ResponseXperpLogin login);
        Task LogOut(string connectionId);
        Task<List<ConncectedUser>> GetConncectedUsers(string code);
        //Task HandleDisconnection(string connectionId);
        //Task<string?> ValidateTokenAsync(string? token);
    }
}
