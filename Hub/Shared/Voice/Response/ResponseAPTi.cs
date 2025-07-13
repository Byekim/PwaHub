using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hub.Shared.Voice.Response
{
    /// <summary>
    /// 푸시 알림톡으로 아파트 아이 api 에 요청했을시에 받을 반환값
    /// </summary>
    public class ResponseAPTi
    {
        public AptIResponseCode code { get; set; }
        public string? description { get; set; }
    }

    /// <summary>
    /// apt i 에서 우리쪽 api 에 요청했을시 줄 반환값 
    /// </summary>
    public class ResponseAPTiBroadcastHistory
    {
        public int historySeq { get; set; }
        public int seq { get; set; }
        public string? aptCd { get; set; }
        public string? title { get; set; }
        public string? body { get; set; }
        public string? speaker { get; set; }
        public string? historyDate { get; set; }
    }


}
