using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hub.Shared.Voice.Response
{
    public class ResponsetHomepageFileUpload : ResponseHomepage
    {
        public ResponsetHomepageFileUploadDetail data { get; set; }
    }

    public class ResponsetHomepageFileUploadDetail
    {
        public string id { get; set; }
        public string title { get; set; }
        public string aptName { get; set; }
        public string aptCd { get; set; }
        public string writer { get; set; }
    }
}
