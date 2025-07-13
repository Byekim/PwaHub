using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hub.Shared
{
    public static class Message
    {
        public static readonly string loginErrorMessage1 = $@"아이디는 영문 소문자 6~10자로 입력해주세요";
        public static readonly string loginErrorMessage2 = $@"존재하지않는 사용자입니다.{Environment.NewLine}아이디를 다시 확인하시거나 회원가입을 진행해주세요.";
        public static readonly string loginErrorMessage3 = @"비밀번호를 다시 확인해주세요";
        public static readonly string loginErrorMessage6 = $@"해당 아이디는 승인 대기 중입니다.{Environment.NewLine}승인 완료 시 알림톡이 발송됩니다.{Environment.NewLine}알림톡 수신 후 다시 로그인해 주세요.";
        public static readonly string xpErpErrorMessage = $@"Xperp에 회원데이터가 없습니다.";

        public static readonly string connectFailMessage = "서버접속에 실패하였습니다";
        public static readonly string unKnownMessage = "서버접속에 실패하였습니다";
        public static readonly string tokenExfirelMessage = "aws contigo token이 만료되었거나 데이터가 없습니다.";
        public static readonly string tokenFailMessage = "aws contigo token을 가져오는데 실패하였습니다.";
        public static readonly string retryMessage = "재시도합니다..";
        public static readonly string deSerialzeFailMessage = "데이터 역질렬화에 실패하였습니다.";

        public static readonly string connectionStopedMessage = "The connection has been stopped.";
        public static readonly string forcedConnectionStopedMessage = "Logout requested by the server.";

    }
}
