using Hub.Server.Models;
using Hub.Server.Services;
using Hub.Shared.Interface;
using Hub.Shared;
using Microsoft.EntityFrameworkCore;
using Hub.Server.Interfaces.Database.Voice;
using Microsoft.EntityFrameworkCore.Storage;
using Hub.Shared.Voice;

namespace Hub.Server.Repository.Voice
{
    public class VoiceBroadcastRepository : iVoiceBroadcastRepository
    {
        private readonly iInMemoryVoiceRepository _inMemoryRepo;
        private readonly iRdbmsVoiceRepository _rdbmsRepo;
        private readonly ILogger<VoiceBroadcastRepository> _logger;

        public VoiceBroadcastRepository(iInMemoryVoiceRepository inMemoryRepo, iRdbmsVoiceRepository rdbmsRepo, ILogger<VoiceBroadcastRepository> logger)
        {
            _inMemoryRepo = inMemoryRepo;
            _rdbmsRepo = rdbmsRepo;
            _logger = logger;
        }

        public async Task<ResultMsgStatus> AddAsync(VoiceBroadCast voiceBroadcast)
        {
            return await _rdbmsRepo.AddAsync(voiceBroadcast);
        }

        public async Task<ResultMsgStatus> DeleteAsync(int seq, string aptCd)
        {
            return await _rdbmsRepo.DeleteAsync(seq, aptCd);
        }

        public async Task<List<VoiceBroadCast>> GetAllAsync(string aptCd)
        {
            return await _rdbmsRepo.GetAllAsync(aptCd);
        }

        public async Task<VoiceBroadCast> GetAsync(int seq, string aptCd)
        {
            return await _rdbmsRepo.GetAsync(seq, aptCd);
        }

        public async Task<ResultMsgStatus> UpdateAsync(VoiceBroadCast voiceBroadcast)
        {
            return await _rdbmsRepo.UpdateAsync(voiceBroadcast);
        }

        #region 예약

        public async Task<ResultMsgStatus> AddReservationAsync(iVoiceReservation reservation)
        {
            if (ResultMsgStatus.OK != await _inMemoryRepo.AddReservationAsync(reservation))
                return ResultMsgStatus.ERROR;
            return await _rdbmsRepo.AddRdbmsReservationAsync(reservation);
        }

        public async Task<ResultMsgStatus> DeleteReservationAsync(iVoiceReservation reservation)
        {
            
            if (ResultMsgStatus.OK != await _inMemoryRepo.DeleteReservationAsync(reservation))
            {
                return ResultMsgStatus.ERROR;
            }
            return await _rdbmsRepo.DeleteRdbmsReservationAsync(reservation);
        }

        public async Task<List<iVoiceReservation>> GetReservationsAsync(string aptCd)
        {
            var inMemoryData = await _inMemoryRepo.GetReservationsAsync(aptCd);
            if (inMemoryData.Any())
                return inMemoryData;
            return await _rdbmsRepo.GetRdbmsReservationsAsync(aptCd);
        }

        public async Task SyncReservationsWithRdbmsAsync()
        {

        }

        public async Task<ResultMsgStatus> DeleteRdbmsReservationAsync(iVoiceReservation reservation)
        {
            using var tran = await _inMemoryRepo.BeginInMemoryTransactionAsync();

            try
            {
                // in-memory 데이터베이스에서 예약 삭제
                ResultMsgStatus resultMsgStatus = await _inMemoryRepo.DeleteReservationAsync(reservation);
                if (resultMsgStatus != ResultMsgStatus.OK)
                {
                    await tran.RollbackAsync();
                    return resultMsgStatus;  // 실패한 경우 바로 반환
                }

                // RDBMS에서 예약 삭제
                resultMsgStatus = await _rdbmsRepo.DeleteRdbmsReservationAsync(reservation);
                if (resultMsgStatus != ResultMsgStatus.OK)
                {
                    await tran.RollbackAsync();
                    return resultMsgStatus;  // 실패한 경우 바로 반환
                }
                await tran.CommitAsync();
                return ResultMsgStatus.OK;  // 모두 성공하면 OK 반환
            }
            catch (Exception ex)
            {
                return ResultMsgStatus.INNER_CATCH_ERROR;  // 모두 성공하면 OK 반환

            }
        }
    

        public Task<ResultMsgStatus> AddRdbmsReservationAsync(iVoiceReservation reservation)
        {
            return _rdbmsRepo.AddRdbmsReservationAsync(reservation);
        }

        public Task<List<iVoiceReservation>> GetRdbmsReservationsAsync(string aptCd)
        {
            return _rdbmsRepo.GetRdbmsReservationsAsync(aptCd);
        }

        #endregion

        #region Voice Group

        public async Task<List<GroupMaster>> GetVoiceGroupAsync(string aptCd)
        {
            return await _rdbmsRepo.GetVoiceGroupAsync(aptCd);
        }

        public async Task<ResultMsgStatus> AddVoiceGroupAsync(GroupMaster groupMaster)
        {
            return await _rdbmsRepo.AddVoiceGroupAsync(groupMaster);
        }

        public async Task<ResultMsgStatus> DelteVoiceGroupAsync(GroupMaster groupMaster)
        {
            return await _rdbmsRepo.DelteVoiceGroupAsync(groupMaster);
        }

        public async Task<IDbContextTransaction> BeginInMemoryTransactionAsync()
        {
            return await _inMemoryRepo.BeginInMemoryTransactionAsync();
        }
        #endregion
    }
}
