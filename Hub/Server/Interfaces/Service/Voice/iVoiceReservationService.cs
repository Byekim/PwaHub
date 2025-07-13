using Hub.Shared.Interface;
using Hub.Shared;

namespace Hub.Server.Interfaces.Service.Voice
{
    public interface iVoiceReservationService
    {
        Task<ResultMsgStatus> DeleteAsync(iVoiceReservation reservation);
        Task<ResultMsgStatus> AddAsync(iVoiceReservation reservation);
        Task<List<iVoiceReservation>> GetReservationsAsync(string aptCd);
        Task SyncReservationsWithRdbmsAsync();
    }
}
