using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hub.Shared.Voice.Response
{
    public class ResponseAPTiCnt : ResponseAPTi
    {
        public ResponseAPTiCntDetail data { get; set; }
    }

    public class ResponseAPTiCntDetail
    {
        public string code { get; set; }
        public string cnt { get; set; }
    }

}
