using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hub.Shared.Interface;

namespace Hub.Shared.Model.Hub.Login
{
    public class ResponseXperpMenuYn : iResponseWithUserId
    {
        /// <summary>
        /// 민원메뉴 0:사용안함 1:읽기/쓰기 2:읽기만
        /// </summary>
        public string jobmAuth { get; set; }
        /// <summary>
        /// isadmin Y/N
        /// </summary>
        public string isadmin { get; set; }
        /// <summary>
        /// 단지관리메뉴 0:사용안함 1:읽기/쓰기 2:읽기만
        /// </summary>
        public string dongAuth { get; set; }
        /// <summary>
        /// 검침메뉴 0:사용안함 1:읽기/쓰기 2:읽기만       
        /// </summary>
        public string inspAuth { get; set; }
        /// <summary>
        /// 부과메뉴 0:사용안함 1:읽기/쓰기 2:읽기만 
        /// </summary>
        public string impoAuth { get; set; }
        /// <summary>
        /// 회계메뉴 0:사용안함 1:읽기/쓰기 2:읽기만
        /// </summary>
        public string acctAuth { get; set; }
        /// <summary>
        /// 사용자이름
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 수납메뉴 0:사용안함 1:읽기/쓰기 2:읽기만
        /// </summary>
        public string recpAuth { get; set; }
        /// <summary>
        /// userId
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// 입주자메뉴 0:사용안함 1:읽기/쓰기 2:읽기만
        /// </summary>
        public string occpAuth { get; set; }
        /// <summary>
        /// 인사급여메뉴 0:사용안함 1:읽기/쓰기 2:읽기만
        /// </summary>
        public string paysAuth { get; set; }
        /// <summary>
        /// 아파트데스크 url
        /// </summary>
        public string url { get; set; }
    }

}
