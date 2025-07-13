using Hub.Shared;
using Hub.Shared.Interface;
using Microsoft.EntityFrameworkCore.Storage;

namespace Hub.Server.Interfaces.Database.Voice
{
    public interface iInMemoryVoiceRepository
    {
        Task<ResultMsgStatus> DeleteReservationAsync(iVoiceReservation reservation);
        Task<ResultMsgStatus> AddReservationAsync(iVoiceReservation reservation);
        Task<List<iVoiceReservation>> GetReservationsAsync(string aptCd);
        Task<IDbContextTransaction> BeginInMemoryTransactionAsync();
        Task SyncReservationsWithRdbmsAsync();
    }
}
