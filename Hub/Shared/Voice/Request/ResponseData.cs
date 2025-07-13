using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hub.Shared.Voice.Request
{
    public class ResponseData
    {
        public ResultMsgStatus result { get; set; }
        public string? data { get; set; }
    }
}
