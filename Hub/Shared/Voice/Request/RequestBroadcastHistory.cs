using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Hub.Shared.Voice.Request
{

    public class RequestBroadcastHistory : RequestBase
    {
        public string? aptCd { get; set; }
        public string? requestDate { get; set; }
        public string? requestCnt { get; set; }
        public string? requestPage { get; set; }
        public string? orderBy { get; set; }
    }

    public class RequestBroadcastAPTI : RequestBase
    {
        public int historySeq { get; set; }
    }

    public class RequestBroadcastSearchByTitle : RequestBroadcastHistory
    {

        public string? keyword { get; set; }

    }
}
