using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hub.Shared.Voice.Response
{
    public class ClovaVoiceErrorResponse
    {
        public string? details { get; set; }
        public string? errorCode { get; set; }
        public string? message { get; set; }
    }
}
