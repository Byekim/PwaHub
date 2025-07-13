using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hub.Shared.Model.Hub.Login
{
    /// <summary>
    /// 프리미엄config 데이터 
    /// </summary>
    public class ResponseXperpPremiumConfig
    {
        public string fireYn { get; set; }      //소방점검사용여부
        public string ttsYn { get; set; }       //TTS사용여부
        public string parkingYn { get; set; }   //방문차량사용여부
        public string aptBankYn { get; set; }   //아파트뱅크사용여부
        public string xpDocYn { get; set; }     //Xp문서함사용여부
        public string useYn { get; set; }       //사용여부
        public string callYn { get; set; }      //통화매니저사용여부
        public string aegisOneYn { get; set; }  //이지스원사용여부
        public string aptCd { get; set; }       //단지코드
        public string electVoteYn { get; set; } //전자투표사용여부
        public string aptNm { get; set; }       //단지명
        public string itProcCom { get; set; }   //전산업체코드
    }
}
