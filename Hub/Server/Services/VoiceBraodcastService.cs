using Hub.Server.Interfaces;
using Hub.Server.Interfaces.Database.Voice;
using Hub.Server.Interfaces.Service.Voice;
using Hub.Server.Models;
using Hub.Shared;
using Hub.Shared.Interface;
using Hub.Shared.Voice;
using Microsoft.AspNetCore.SignalR;

namespace Hub.Server.Services
{

    public class VoiceBraodcastService : iVoiceBroadcastService
    {

        private readonly iVoiceBroadcastRepository _repository;

        public VoiceBraodcastService(iVoiceBroadcastRepository repository)
        {
            _repository = repository;
        }

        public Task<ResultMsgStatus> AddVoiceBroadcast(VoiceBroadCast voiceBroadcast)
        {
            return _repository.AddAsync(voiceBroadcast);
        }

        public Task<ResultMsgStatus> DeletevoiceBroadcast(VoiceBroadCast pVoiceBroadcast)
        {
            return _repository.DeleteAsync(pVoiceBroadcast.seq,pVoiceBroadcast.aptCd);
        }

        public Task<List<VoiceBroadCast>> GetAllVoiceBroadcast(string aptCd)
        {
            return _repository.GetAllAsync(aptCd);
        }

        public Task<VoiceBroadCast> GetVoiceBroadcast(int seq, string aptCd)
        {
            return _repository.GetAsync(seq, aptCd);
        }

        public Task<ResultMsgStatus> UpdateVoiceBroadcast(VoiceBroadCast voiceBroadcast)
        {
            return _repository.UpdateAsync(voiceBroadcast);
        }

        public Task<List<GroupMaster>> GetVoiceGroup(string aptCd)
        {
            return _repository.GetVoiceGroupAsync(aptCd);
        }


        public Task<ResultMsgStatus> SetVoiceGroup(GroupMaster groupMaster)
        {
            return _repository.AddVoiceGroupAsync(groupMaster);
        }
    }
}
