using Hub.Shared.Voice;
using System;

namespace Hub.Shared.Voice.Request
{

    public class RequestXPVoiceAuth : RequestBase
    {
        public string? aptCd { get; set; }
        public string? ip { get; set; }
        public Grp grp { get; set; }
        public string? inputUser { get; set; }
    }
    public class RequestVoiceNotice : VoiceNotice
    {
        public string? aptCd { get; set; }
        public string? ip { get; set; }
        public string? inputUser { get; set; }
    }


}
