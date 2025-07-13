using Hub.Shared;
using Hub.Shared.Interface;
using Hub.Shared.Voice;

namespace Hub.Server.Interfaces.Database.Voice
{
    public interface iRdbmsVoiceRepository
    {
        /// <summary>
        /// 방송목록
        /// </summary>
        /// <param name="voiceBroadcast"></param>
        /// <returns></returns>
        Task<ResultMsgStatus> AddAsync(VoiceBroadCast voiceBroadcast);
        Task<ResultMsgStatus> UpdateAsync(VoiceBroadCast voiceBroadcast);
        Task<ResultMsgStatus> DeleteAsync(int seq, string aptCd);
        Task<VoiceBroadCast> GetAsync(int seq, string aptCd);
        Task<List<VoiceBroadCast>> GetAllAsync(string aptCd);

        /// <summary>
        /// Group
        /// </summary>
        /// <param name="aptCd"></param>
        /// <returns></returns>
        Task<List<GroupMaster>> GetVoiceGroupAsync(string aptCd);
        Task<ResultMsgStatus> AddVoiceGroupAsync(GroupMaster groupMaster);
        Task<ResultMsgStatus> DelteVoiceGroupAsync(GroupMaster groupMaster);

        /// <summary>
        /// 예약
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        Task<ResultMsgStatus> DeleteRdbmsReservationAsync(iVoiceReservation reservation);
        Task<ResultMsgStatus> AddRdbmsReservationAsync(iVoiceReservation reservation);
        Task<List<iVoiceReservation>> GetRdbmsReservationsAsync(string aptCd);

    }
}
