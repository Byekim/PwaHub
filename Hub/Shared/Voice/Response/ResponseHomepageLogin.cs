using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hub.Shared.Voice.Response
{
    public class ResponseHomepageLogin : ResponseHomepage
    {
        public ResponseHomepageLoginDetail data { get; set; }
    }

    public class ResponseHomepageLoginDetail
    {
        public string name { get; set; }
        public string type { get; set; }
        public string token { get; set; }
    }
}
