using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hub.Shared.Voice.Request
{
    public class RequestVoice
    {
        public string? aptCd { get; set; }
        public string? id { get; set; }
        public int seq { get; set; }
        public BroadcastType broadcastType { get; set; }
    }
}
