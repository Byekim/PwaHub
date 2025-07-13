using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hub.Shared.Interface;

namespace Hub.Shared.Model.Hub
{
    public class User
    {
        public string userId { get; set; }
        public string userPw { get; set; }
        public string emailAddress { get; set; }
    }

    public class RequestUser : User, iResponseWithUserId
    {

    }

    public class UserToken
    {
        public int seq { get; set; }
        public string userId { get; set; }
        public string token { get; set; }
        public DateTime expiryDate { get; set; }
        public DateTime createdAt { get; set; }
    }




}
