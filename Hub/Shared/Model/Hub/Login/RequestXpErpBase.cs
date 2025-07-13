using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hub.Shared.Model.Hub.Login
{
    public class RequestXpErpBase
    {
        public string userId { get; set; }
        public string? aptCd { get; set; }
        public string passWord { get; set; }
    }
}
