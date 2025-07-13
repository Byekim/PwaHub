using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Hub.Shared.Voice
{

    public class VoiceBroadcastGroup
    {
        [Required]
        public int seq { get; set; }
        public int groupSeq { get; set; }  // groupMaster의 groupSeq 속성
        public string aptCd { get; set; }  // groupMaster의 aptCd 속성
        public GroupMaster groupMaster { get; set; }

    }

    public class GroupMaster
    {
        //[Required]
        public int groupSeq { get; set; }
        [Required]
        public string aptCd { get; set; }
        //[Required]
        public string yn { get; set; }
        //[Required]
        public string? groupName { get; set; }


    }
}
