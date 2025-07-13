
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hub.Shared.Voice
{

    public class BroadCast
    {
        /// <summary>
        /// 제목
        /// </summary>        
        public string? title { get; set; }
        /// <summary>
        /// 성우 목소리
        /// </summary>
        public string? speaker { get; set; }
        /// <summary>
        /// 담당자
        /// </summary>
        public string? person { get; set; }
        /// <summary>
        /// 등록일자
        /// </summary>
        public string? inputDay { get; set; }
        /// <summary>
        /// 등록시간
        /// </summary>
        public string? inputTime { get; set; }
        /// <summary>
        /// 자주찾기
        /// </summary>
        public string? bookMark { get; set; }
    }



    public class VoiceBroadCast : BroadCast
    {
        public int number { get; set; }
        public int seq { get; set; }
        public string? aptCd { get; set; }
        public string body { get; set; }
        public string? use { get; set; }
        public string? voiceSpeed { get; set; }
        public string? firstMent { get; set; }
        public string? middleMent { get; set; }
        public string? endMent { get; set; }
        //public Ment? Ment { get; set; }
        public string? id { get; set; }
        public bool aptI { get; set; }
        public string? backGroudSound { get; set; }
        public string? modifyDate { get; set; }
        public string? inputDate { get; set; }
        public string? broadcastType { get; set; }
        public int groupSeq { get; set; }
        public virtual VoiceBroadcastGroup? voiceGroup { get; set; }
    }

    public class VoiceBroadCastHistory : BroadCast
    {
        public int historySeq { get; set; }
        public int number { get; set; }
        public int seq { get; set; }
        public string? aptCd { get; set; }
        public string body { get; set; }
        public string? use { get; set; }
        public string? voiceSpeed { get; set; }
        public string? firstMent { get; set; }
        public string? middleMent { get; set; }
        public string? endMent { get; set; }
        //public Ment? Ment { get; set; }
        public string? id { get; set; }
        public bool aptI { get; set; }
        public string? backGroudSound { get; set; }
        public string? modifyDate { get; set; }
        public string? inputDate { get; set; }
        public string? broadcastType { get; set; }
        public virtual VoiceBroadcastGroup? voiceGroup { get; set; }

    }
}
