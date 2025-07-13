using Hub.Shared.Model.Hub.Login;
using Hub.Shared;
using Microsoft.AspNetCore.Mvc;
using NetworkLibrary;
using Hub.Shared.Interface;
using Hub.Server.Interfaces.Service.Voice;
using Hub.Shared.Voice;
using Hub.Shared.Voice.Request;

namespace Hub.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoiceController : ControllerBase
    {
        private readonly iVoiceBroadcastService _ivoiceBroadcastService;
        private readonly iVoiceReservationService _ivoiceReservationService;
        public VoiceController(iVoiceBroadcastService iVoice, iVoiceReservationService voiceReservationService)
        {
            this._ivoiceBroadcastService = iVoice;
            this._ivoiceReservationService = voiceReservationService;
        }

        #region 방송
        /// <summary>
        /// 방송목록 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetAllVoiceBroadcast")]
        public async Task<List<VoiceBroadCast>?> GetAllVoiceBroadcast(RequestVoice requestVoice)
        {
            
            return await _ivoiceBroadcastService.GetAllVoiceBroadcast(requestVoice.aptCd);
        }

        /// <summary>
        /// 방송상세 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetVoiceBroadcast")]
        public async Task<VoiceBroadCast>? GetVoiceBroadcast(string request)
        {
            RequestVoice requestVoice = request.DecryptData<RequestVoice>();
            if (requestVoice?.aptCd == null)
                return null;
            return await _ivoiceBroadcastService.GetVoiceBroadcast(requestVoice.seq, requestVoice.aptCd);
        }

        /// <summary>
        /// 방송등록
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("AddVoiceBroadcast")]
        public async Task<ResultMsgStatus> AddVoiceBroadcast(string request)
        {
            VoiceBroadCast voiceBroadCast = request.DecryptData<VoiceBroadCast>();
            if (voiceBroadCast?.aptCd == null)
                return ResultMsgStatus.FORMATTER_ERROR;
            return await _ivoiceBroadcastService.AddVoiceBroadcast(voiceBroadCast);
        }

        /// <summary>
        /// 방송수정
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("UpdateVoiceBroadcast")]
        public async Task<ResultMsgStatus> UpdateVoiceBroadcast(string request)
        {
            VoiceBroadCast voiceBroadCast = request.DecryptData<VoiceBroadCast>();
            if (voiceBroadCast?.aptCd == null)
                return ResultMsgStatus.FORMATTER_ERROR;
            return await _ivoiceBroadcastService.UpdateVoiceBroadcast(voiceBroadCast);
        }

        /// <summary>
        /// 방송삭제
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("DeleteVoiceBroadcast")]
        public async Task<ResultMsgStatus> DeletevoiceBroadcast(string request)
        {
            VoiceBroadCast voiceBroadCast = request.DecryptData<VoiceBroadCast>();
            if (voiceBroadCast?.aptCd == null)
                return ResultMsgStatus.FORMATTER_ERROR;
            return await _ivoiceBroadcastService.DeletevoiceBroadcast(voiceBroadCast);
        }
        #endregion

        #region 예약
        [HttpPost("AddReservation")]
        public async Task<ResultMsgStatus> AddReservation(string request)
        {
            var voiceBroadCast = request.DecryptData<iVoiceReservation>();
            if (voiceBroadCast?.aptCd == null)
                return ResultMsgStatus.FORMATTER_ERROR;
            return await _ivoiceReservationService.AddAsync(voiceBroadCast);
        }

        [HttpPost("DeleteReservation")]
        public async Task<ResultMsgStatus> DeleteReservation(string request)
        {
            var voiceBroadCast = request.DecryptData<iVoiceReservation>();
            if (voiceBroadCast?.aptCd == null)
                return ResultMsgStatus.FORMATTER_ERROR;
            return await _ivoiceReservationService.DeleteAsync(voiceBroadCast);
        }

        [HttpPost("GetReservation")]
        public async Task<List<iVoiceReservation>> GetReservation(string request)
        {
            RequestVoice requestVoice = request.DecryptData<RequestVoice>();
            if (requestVoice?.aptCd == null)
                return null;
            return await _ivoiceReservationService.GetReservationsAsync(requestVoice.aptCd);
        }
        #endregion


    }
}
