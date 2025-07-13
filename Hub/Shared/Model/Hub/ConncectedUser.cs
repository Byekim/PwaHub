using Hub.Shared.Model.Hub.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hub.Shared.Model.Hub
{
    public  class ConncectedUser
    {
        public DateTime connctedTime { get; set; }
        public string connectionId { get; set; }
        public string groupName { get; set; }
        public ResponseXperpLogin xpErpUserData { get; set; }

    }
}
