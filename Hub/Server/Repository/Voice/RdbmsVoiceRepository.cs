using Hub.Server.Interfaces.Database.Voice;
using Hub.Server.Models;
using Hub.Shared;
using Hub.Shared.Interface;
using Hub.Shared.Voice;
using Microsoft.EntityFrameworkCore;
using NetworkLibrary;

namespace Hub.Server.Repository.Voice
{
    public class RdbmsVoiceRepository : iRdbmsVoiceRepository
    {
        private readonly XpVoiceDbContext _dbContext;
        private readonly ILogger<RdbmsVoiceRepository> _logger;

        public RdbmsVoiceRepository(XpVoiceDbContext dbContext, ILogger<RdbmsVoiceRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ResultMsgStatus> AddAsync(VoiceBroadCast voiceBroadcast)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            ResultMsgStatus resultMsgStatus = ResultMsgStatus.ERROR;
            try
            {
                //같은 아파트코드에 seq값이 동일하다면 update 처리
                var temp = await _dbContext.voiceBroadCasts.Where(r => r.seq == voiceBroadcast.seq && r.aptCd == voiceBroadcast.aptCd).ToListAsync();
                if (temp.Count > 0)
                {
                    _dbContext.voiceBroadCasts.Update(voiceBroadcast);
                }
                else
                {
                    await _dbContext.voiceBroadCasts.AddAsync(voiceBroadcast);
                }
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                resultMsgStatus = ResultMsgStatus.OK;
            }
            catch (Exception ex)
            {
                //롤백
                await transaction.RollbackAsync();
                _logger.LogError(ex, "AddAsync");
                string message = $@"Error in AddAsync: {ex.Message}";
                await TelegramService.Instance.SendMessageAsync(GlobalVariable.telegramChatId, message);
                resultMsgStatus = ResultMsgStatus.INNER_CATCH_ERROR;
            }
            return resultMsgStatus;
        }

        public async Task<ResultMsgStatus> DeleteAsync(int seq, string aptCd)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            ResultMsgStatus resultMsgStatus = ResultMsgStatus.ERROR;

            try
            {
                VoiceBroadCast? voiceBroadCast = await _dbContext.voiceBroadCasts.Where(r => r.seq == seq && r.aptCd == aptCd).FirstOrDefaultAsync();
                if (voiceBroadCast != null)
                {
                    _dbContext.voiceBroadCasts.Remove(voiceBroadCast);
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    resultMsgStatus = ResultMsgStatus.OK;
                }
                else
                {
                    throw new ArgumentNullException();
                }

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error in DeletevoiceBroadcast");
                string message = $@"Error in DeletevoiceBroadcast: {ex.Message}";
                await TelegramService.Instance.SendMessageAsync(GlobalVariable.telegramChatId, message);
                resultMsgStatus = ResultMsgStatus.INNER_CATCH_ERROR;
            }
            return resultMsgStatus;
        }



        public async Task<ResultMsgStatus> UpdateAsync(VoiceBroadCast voiceBroadcast)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            ResultMsgStatus resultMsgStatus = ResultMsgStatus.ERROR;
            try
            {
                VoiceBroadCast? voiceBroadCast = await _dbContext.voiceBroadCasts.Where(r => r.seq == voiceBroadcast.seq && r.aptCd == voiceBroadcast.aptCd).FirstOrDefaultAsync();
                if (voiceBroadCast != null)
                {
                    _dbContext.voiceBroadCasts.Update(voiceBroadcast);
                }
                else
                {
                    throw new ArgumentNullException();
                }
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                resultMsgStatus = ResultMsgStatus.OK;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error in UpdateVoiceBroadcast");
                string message = $@"Error in UpdateVoiceBroadcast: {ex.Message}";
                await TelegramService.Instance.SendMessageAsync(GlobalVariable.telegramChatId, message);
                resultMsgStatus = ResultMsgStatus.INNER_CATCH_ERROR;
            }
            return resultMsgStatus;
        }

        public async Task<List<VoiceBroadCast>> GetAllAsync(string aptCd)
        {
            if (aptCd == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                return await _dbContext.voiceBroadCasts.Where(v => v.aptCd == aptCd).ToListAsync();
            }
        }

        public async Task<VoiceBroadCast> GetAsync(int seq, string aptCd)
        {
            if (aptCd == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                return await _dbContext.voiceBroadCasts.Where(v => v.aptCd == aptCd && v.seq == seq).FirstOrDefaultAsync();
            }
        }

        public async Task<ResultMsgStatus> AddRdbmsReservationAsync(iVoiceReservation reservation)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            ResultMsgStatus resultMsgStatus = ResultMsgStatus.ERROR;
            try
            {
                await _dbContext.AddAsync(reservation);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
                resultMsgStatus = ResultMsgStatus.OK;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error in UpdateVoiceBroadcast");
                string message = $@"Error in UpdateVoiceBroadcast: {ex.Message}";
                await TelegramService.Instance.SendMessageAsync(GlobalVariable.telegramChatId, message);
                resultMsgStatus = ResultMsgStatus.INNER_CATCH_ERROR;
            }
            return resultMsgStatus;
        }

        public async Task<ResultMsgStatus> DeleteRdbmsReservationAsync(iVoiceReservation reservation)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            ResultMsgStatus resultMsgStatus = ResultMsgStatus.ERROR;
            try
            {
                _dbContext.Remove(reservation);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
                resultMsgStatus = ResultMsgStatus.OK;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error in UpdateVoiceBroadcast");
                string message = $@"Error in UpdateVoiceBroadcast: {ex.Message}";
                await TelegramService.Instance.SendMessageAsync(GlobalVariable.telegramChatId, message);
                resultMsgStatus = ResultMsgStatus.INNER_CATCH_ERROR;
            }
            return resultMsgStatus;
        }


        public async Task<List<iVoiceReservation>> GetRdbmsReservationsAsync(string aptCd)
        {
            var result = await _dbContext.generalReservations.Where(r => r.aptCd == aptCd && r.reservationTime > DateTime.Now).ToListAsync();
            var scheduledReservations = await _dbContext.schuledReservations.Where(r => r.aptCd == aptCd).ToListAsync();
            var periodReservations = await _dbContext.periodReservations.Where(p => p.aptCd == aptCd && p.startDate <= DateTime.Now && p.endDate >= DateTime.Now).ToListAsync();
            return result.Cast<iVoiceReservation>().Concat(scheduledReservations).Concat(periodReservations).ToList();
        }


        public async Task<List<GroupMaster>> GetVoiceGroupAsync(string aptCd)
        {
            if (aptCd == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                return await _dbContext.groupMasters.Where(v => v.aptCd == aptCd).ToListAsync();
            }
        }
        public async Task<ResultMsgStatus> AddVoiceGroupAsync(GroupMaster groupMaster)
        {
            ResultMsgStatus resultMsgStatus = ResultMsgStatus.ERROR;
            try
            {
                if(_dbContext.groupMasters.Count() > 0)
                {
                    var temp = await _dbContext.groupMasters.Where(r => r.groupSeq == groupMaster.groupSeq && r.aptCd == groupMaster.aptCd).DefaultIfEmpty().ToListAsync();
                    if (temp.Count > 0)
                    {
                        _dbContext.groupMasters.Update(groupMaster);
                    }
                    else
                    {
                        await _dbContext.groupMasters.AddAsync(groupMaster);
                    }
                }
                else
                {
                    await _dbContext.groupMasters.AddAsync(groupMaster);
                }
                
                await _dbContext.SaveChangesAsync();
                resultMsgStatus = ResultMsgStatus.OK;
            }
            catch (Exception ex)
            {
                //롤백
                _logger.LogError(ex, "AddVoiceGroupAsync");
                string message = $@"Error in AddVoiceGroupAsync: {ex.Message}";
                await TelegramService.Instance.SendMessageAsync(GlobalVariable.telegramChatId, message);
                resultMsgStatus = ResultMsgStatus.INNER_CATCH_ERROR;
            }
            return resultMsgStatus;

        }

        public async Task<ResultMsgStatus> DelteVoiceGroupAsync(GroupMaster groupMaster)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            ResultMsgStatus resultMsgStatus = ResultMsgStatus.ERROR;
            try
            {
                _dbContext.groupMasters.Remove(groupMaster);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
                resultMsgStatus = ResultMsgStatus.OK;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error in DelteVoiceGroupAsync");
                string message = $@"Error in DelteVoiceGroupAsync: {ex.Message}";
                await TelegramService.Instance.SendMessageAsync(GlobalVariable.telegramChatId, message);
                resultMsgStatus = ResultMsgStatus.INNER_CATCH_ERROR;
            }
            return resultMsgStatus;
        }

        public async Task SyncReservationsAsync() { }
    }
}
