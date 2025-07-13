using Hub.Server.Interfaces.Database.Voice;
using Hub.Server.Interfaces.Service.Voice;
using Hub.Shared;
using Hub.Shared.Interface;

namespace Hub.Server.Services
{
    public class VoiceReservationService : iVoiceReservationService
    {
        private readonly iVoiceBroadcastRepository _repository;
        
        public VoiceReservationService(iVoiceBroadcastRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResultMsgStatus> AddAsync(iVoiceReservation reservation)
        {
            return await _repository.AddReservationAsync(reservation);
        }

        public async Task<ResultMsgStatus> DeleteAsync(iVoiceReservation reservation)
        {
            return await _repository.DeleteReservationAsync(reservation);
        }

        public async Task<List<iVoiceReservation>> GetReservationsAsync(string aptCd)
        {
            return await _repository.GetReservationsAsync(aptCd);
        }

        public async Task SyncReservationsWithRdbmsAsync()
        {
            
        }
    }
}
