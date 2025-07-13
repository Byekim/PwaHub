using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hub.Shared.Interface;

namespace Hub.Shared.Model.Hub.Login
{
    /// <summary>
    /// 로그인
    /// </summary>
    public class ResponseXperpLogin : User, iResponseWithUserId
    {
        //public string userId { get; set; }
        public string aptCd { get; set; }
        public string useAuth { get; set; }
        public string reqNm { get; set; }
        public string dbLink { get; set; }
        public string userPw { get; set; }
        //public List<TdgIpList> tdgIpListBatchResponseList { get; set; }
        public string useYn { get; set; } //사용여부 (T:사용,H:비번3번 틀렸을때,N:사용안함)
        public string reqDivision { get; set; } //신청구분 (R:퇴직,O:재입사,C:개정전환,N:신규계정,S:휴먼해제신청중,A:아이디관리자신청중,U:메뉴수정요청,' ':사용상태
        public string failCnt { get; set; }
        public string approval { get; set; } // 승인(1차승인1,2차승인2)
    }

}
