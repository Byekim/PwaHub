
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hub.Shared.Voice
{
    public class MostBroadcastHistory : VoiceBroadCast
    {

        public string? startDate { get; set; }
        public string? endDate { get; set; }
        /// <summary>
        /// 방송횟수
        /// </summary>
        public string? cnt { get; set; }
    }
}
