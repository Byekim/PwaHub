using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace Hub.Shared.Voice.Request
{
    [Serializable]
    public class RequestHomepageFileUpload<T>
    {
        public string? title { get; set; }
        public string? aptName { get; set; }
        public string? aptCd { get; set; }
        public string? writer { get; set; }
        public string? writerPassword { get; set; }
        public T? file { get; set; }
    }
}
