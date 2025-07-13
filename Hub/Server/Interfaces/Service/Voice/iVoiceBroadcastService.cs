using Hub.Shared;
using Hub.Shared.Voice;

namespace Hub.Server.Interfaces.Service.Voice
{
    /// <summary>
    /// port 
    /// </summary>

    public interface iVoiceBroadcastService
    {
        public Task<ResultMsgStatus> AddVoiceBroadcast(VoiceBroadCast voiceBroadcast);

        public Task<ResultMsgStatus> UpdateVoiceBroadcast(VoiceBroadCast voiceBroadcast);

        public Task<ResultMsgStatus> DeletevoiceBroadcast(VoiceBroadCast pVoiceBroadcast);

        public Task<VoiceBroadCast> GetVoiceBroadcast(int seq, string aptCd);

        public Task<List<VoiceBroadCast>> GetAllVoiceBroadcast(string aptCd);

        public Task<List<GroupMaster>> GetVoiceGroup(string aptCd);

        public Task<ResultMsgStatus> SetVoiceGroup(GroupMaster groupMaster);
    }
}
