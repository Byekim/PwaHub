using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hub.Shared.Model.Hub
{
    public class XpHubUserGroup
    {
        [Key]
        [Column("GROUP_SEQ")]
        public string groupSeq { get; set; } = null!;

        [Key]
        [Column("GROUP_ID")]
        public string groupId { get; set; } = null!;

        [Key]
        [Column("SEQ")]
        public string seq { get; set; } = null!;

        [Key]
        [Column("ID")]
        public string id { get; set; } = null!;

        [Column("INPUT_DATE")]
        public DateTime? inputDate { get; set; }

        [Column("INPUT_ID")]
        public string? inputId { get; set; }

        [Column("MODIFY_DATE")]
        public DateTime? modifyDate { get; set; }

        [Column("MODIFY_ID")]
        public string? modifyId { get; set; }

    }


    public class ResponseXpHubUserGroup: XpHubUserGroup
    {

       public List<SiteAddress> siteAddressList { get; set; } = new List<SiteAddress>();

    }
}
