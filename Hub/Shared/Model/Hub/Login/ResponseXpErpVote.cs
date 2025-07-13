using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hub.Shared.Interface;

namespace Hub.Shared.Model.Hub.Login
{
    public class ResponseXpErpVote : iResponseWithUserId
    {
        /// <summary>
        /// 권한 그룹코드
        /// </summary>
        public string authGrpCd { get; set; }
        public string userId { get; set; }
        public string aptCd { get; set; }
        /// <summary>
        ///  버튼권한 (F:읽기/쓰기,T:읽기)        
        /// </summary>
        public string btnRead { get; set; }
    }
}
