
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hub.Shared.Voice.Request
{
    /// <summary>
    /// apt i 에 api  요청했을시 줄 요청값
    /// </summary>

    public class RequestAPTi
    {
        public string api_key { get; set; }
        public string code { get; set; }
        public string subject { get; set; }
        public int voice_idx { get; set; }
    }
}
