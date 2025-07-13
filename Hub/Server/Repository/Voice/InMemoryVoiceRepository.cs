using Hub.Server.Interfaces.Database.Voice;
using Hub.Server.Models;
using Hub.Shared;
using Hub.Shared.Interface;
using Hub.Shared.Voice.ReservationHandler;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using NetworkLibrary;
using StackExchange.Redis;

namespace Hub.Server.Repository.Voice
{
    public class InMemoryVoiceRepository : iInMemoryVoiceRepository
    {
        private readonly XpVoiceDbContext _inMemoryDbContext;
        private readonly ILogger<InMemoryVoiceRepository> _logger;

        public InMemoryVoiceRepository(XpVoiceDbContext inMemoryDbContext, ILogger<InMemoryVoiceRepository> logger)
        {
            _inMemoryDbContext = inMemoryDbContext;
            _logger = logger;
        }

        public async Task<ResultMsgStatus> AddReservationAsync(iVoiceReservation reservation)
        {
            ResultMsgStatus resultMsgStatus = ResultMsgStatus.ERROR;
            try
            {
                var dbSet = GetDbSetForReservation(reservation);
                if (dbSet == null)
                {
                    throw new InvalidOperationException($"Unsupported reservation type: {reservation.GetType().Name}");
                }
                if (ResultMsgStatus.OK != await ProcessReservationAsync(reservation, ReservationMethodType.Add))
                {
                    throw new InvalidOperationException($"ProcessReservationAsync: {reservation.GetType().Name}");
                }
                resultMsgStatus = ResultMsgStatus.OK;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddAsync");
                string message = $@"Error in AddAsync: {ex.Message}";
                await TelegramService.Instance.SendMessageAsync(GlobalVariable.telegramChatId, message);
                resultMsgStatus = ResultMsgStatus.INNER_CATCH_ERROR;
            }
            return resultMsgStatus;
        }
        public async Task<IDbContextTransaction> BeginInMemoryTransactionAsync()
        {
            // in-memory에서 트랜잭션을 시작하고 이를 반환합니다.
            var transaction = await _inMemoryDbContext.Database.BeginTransactionAsync();
            return transaction;
        }


        public async Task<ResultMsgStatus> DeleteReservationAsync(iVoiceReservation reservation)
        {
            ResultMsgStatus resultMsgStatus = ResultMsgStatus.ERROR;
            
            try
            {
                var dbSet = GetDbSetForReservation(reservation);
                if (dbSet == null)
                {
                    throw new InvalidOperationException($"Unsupported reservation type: {reservation.GetType().Name}");
                }
                if (ResultMsgStatus.OK != await ProcessReservationAsync(reservation, ReservationMethodType.Remove))
                {
                    throw new InvalidOperationException($"ProcessReservationAsync: {reservation.GetType().Name}");
                }
                resultMsgStatus = ResultMsgStatus.OK;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteAsync");
                string message = $@"Error in DeleteAsync: {ex.Message}";
                await TelegramService.Instance.SendMessageAsync(GlobalVariable.telegramChatId, message);
                resultMsgStatus = ResultMsgStatus.INNER_CATCH_ERROR;
            }
            return resultMsgStatus;
        }

        public async Task<List<iVoiceReservation>> GetReservationsAsync(string aptCd)
        {
            var result = await _inMemoryDbContext.generalReservations.Where(r => r.aptCd == aptCd && r.reservationTime > DateTime.Now).ToListAsync();
            var scheduledReservations = await _inMemoryDbContext.schuledReservations.Where(r => r.aptCd == aptCd).ToListAsync();
            var periodReservations = await _inMemoryDbContext.periodReservations.Where(p => p.aptCd == aptCd && p.startDate <= DateTime.Now && p.endDate >= DateTime.Now).ToListAsync();
            return result.Cast<iVoiceReservation>().Concat(scheduledReservations).Concat(periodReservations).ToList();
        }

        public async Task SyncReservationsWithRdbmsAsync()
        {
        }

        private DbSet<iVoiceReservation>? GetDbSetForReservation(iVoiceReservation reservation)
        {
            var method = typeof(XpVoiceDbContext).GetMethod(nameof(XpVoiceDbContext.Set), Type.EmptyTypes);
            var genericMethod = method?.MakeGenericMethod(reservation.GetType());
            return genericMethod?.Invoke(_inMemoryDbContext, null) as DbSet<iVoiceReservation>;
        }

        private async Task<ResultMsgStatus> ProcessReservationAsync(iVoiceReservation reservation, ReservationMethodType reservationMethodType)
        {
            var dbSetMapping = new Dictionary<Type, Func<Task>>
    {
        { typeof(GeneralReservation), async () => await _inMemoryDbContext.generalReservations.AddAsync((GeneralReservation)reservation) },
        { typeof(ScheduledReservation), async () => await _inMemoryDbContext.schuledReservations.AddAsync((ScheduledReservation)reservation) },
        { typeof(PeriodReservation), async () => await _inMemoryDbContext.periodReservations.AddAsync((PeriodReservation)reservation) }
    };

            if (await _inMemoryDbContext.Set<iVoiceReservation>().AnyAsync(r => r.ConflictsWith(reservation)) && reservationMethodType == ReservationMethodType.Add)
            {
                return ResultMsgStatus.ALREADY;
            }

            if (dbSetMapping.TryGetValue(reservation.GetType(), out var addReservation))
            {
                if (reservationMethodType == ReservationMethodType.Add)
                {
                    await addReservation();
                }
                else if (reservationMethodType == ReservationMethodType.Remove)
                {
                    var entity = await _inMemoryDbContext.Set<iVoiceReservation>().FindAsync(reservation.seq);
                    if (entity != null)
                    {
                        _inMemoryDbContext.Set<iVoiceReservation>().Remove(entity);
                    }
                }
                return ResultMsgStatus.OK;
            }

            return ResultMsgStatus.ERROR;
        }



    }
}
