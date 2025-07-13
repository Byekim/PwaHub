using Hub.Shared.Model.Hub.Login;
using Hub.Shared.Voice;

namespace Hub.Server.Interfaces
{
    public interface iNotifiCationClient
    {
        Task ReceiveNotification(string message);
        Task JoinGroupByCode(string code);
        Task Login(ResponseXperpLogin login);  
        Task LeaveGroupByCode(string code);
        Task SessionEnded(string message);

        /*
        Task ReceiveReservation(Reservation reservation);
        Task ReceiveReservations(List<Reservation> reservations);*/
    }
}
