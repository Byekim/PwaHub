
using System.ComponentModel;


namespace Hub.Shared
{
    #region Voice
    /// <summary>
    /// Speaker 목소리
    /// </summary>
    public enum Speaker
    {
        
        [Description("다인[아동]NEW")]
        vdain,
        [Description("민정[여]NEW")]
        nminjeong,
        [Description("아라[여]추천")]
        dara,
        [Description("유진[여]추천")]
        nyujin,
        [Description("지윤[여]추천")]
        njiyun,
        [Description("기서[여]")]
        ntiffany,
        [Description("나라[여]")]
        nara,
        [Description("민서[여]")]
        nminseo,
        [Description("늘봄[여]")]
        napple,
        [Description("지훈[남]")]
        njihun,
        [Description("미진[여]")]
        mijin,
        [Description("태진[남]")]
        ntaejin,
        [Description("진호[남]")]
        jinho,
        [Description("안나[영어]")]
        danna,
        [Description("차화[중국어]")]
        chiahua,
        [Description("안나[한/영어]")]
        dara_danna,
    }

    public enum MostTop5Type
    {
        DAILY,
        MONTHILY,
        QUARTORLY,

    }

    public enum ReservationMethodType
    {
        Add,
        Remove,
    }

    public enum OnOff
    {
        ON,
        OFF,
    }
    public enum Chime
    {
        [Description("시작음 1")]
        chime1,
        [Description("클래식 1")]
        chime2,
        [Description("클래식 2")]
        chime3,
        [Description("클래식 3")]
        chime4,
        [Description("클래식 4")]
        chime5,
        [Description("종료음 1")]
        chime6,
        [Description("클래식 5")]
        chime7,
        [Description("클래식 6")]
        chime8,
        [Description("클래식 7")]
        chime9,
        [Description("클래식 8")]
        chime10,
        [Description("클래식 9")]
        chime11,
        [Description("시작음 2")]
        chime12,
        [Description("시작음 3")]
        chime13,
        [Description("종료음 2")]
        chime14,
        [Description("종료음 3")]
        chime15,
    }
    public enum MusicType
    {
        M,
        L,
        E,
    }
    public enum VoicePlayType
    {
        MUSIC,
        BROADCAST,
        RESERVATIION,
        NONE,
    }

    public enum ReservationType
    {
        [Description("일반예약")]
        NORMAL,
        [Description("정기예약")]
        ROUTINE,
        [Description("기간예약")]
        FROMTO,
    }

    public enum Grade
    {
        [Description("A00001")]
        NORMAL,
        [Description("A00002")]
        RARE,
        [Description("A00003")]
        MAGIC,
        [Description("A00004")]
        UNIQUE,
    }

    public enum BroadcastType
    {
        NORMAL,
        EMERGENCY,
        CALAMITY,
    }

    public enum AptIResponseCode : int
    {
        [Description("성공")]
        Success = 0,
        [Description("음성방송 사용중인 단지입니다.")]
        FailAlready = 20101,
        [Description("음성방송 사용중이지 않은 단지입니다")]
        FailNotUseVoice = 20102,
        [Description("음성방송 메뉴 오픈에 실패하였습니다.")]
        FailCanNotOpenMenu = 20103,
        [Description("음성방송 메뉴 오픈에 실패하였습니다.")]
        FailCanNotOpenMenuTwo = 20104,
        [Description("등록되어 있는 회원이 없습니다.")]
        FailNoMember = 20105,
        [Description("음성방송 등록에 실패하였습니다.")]
        FailCanNotRegisterVoice = 20106,
        [Description("단지코드를 확인해주시기 바랍니다.")]
        FailStrangeAptCd = 20204
    }
    #endregion

    public enum MessageType
    {
        Alert,
        Update,
        Error,
        SessionStart,
        SessionStop,
        SessionForceEnd,
    }

    public enum XpHubApiStatus
    {
        SUCCESS,
        FAIL,
    }

    public enum DBLink
    {
        XPRO, YPRO, KPRO, IPRO
    }

    public enum AdditionalServiceName
    {
        XpVoice,
        XpCti,
        XpApproval,
        XpDoc,
        XpVote,
        XpBanking,
        AptDesk,
        /// <summary>
        /// 전자세금계산서
        /// </summary>
        BillMate,
        /// <summary>
        /// 세무주치의
        /// </summary>
        AptTax,
        /// <summary>
        /// 회계메이트
        /// </summary>
        AptAudit
    }

    public enum Tier
    {
        /// <summary>
        /// 
        /// </summary>
        Tier1,
        Tier2,
    }

    public enum SubServiceName
    {
        XpVoice,
        XpCti,
        XpVoi,
        XpDoc,
        XpVote,
        XpBanking
    }



    public enum XpErpUserStatus
    {
        /// <summary>
        /// 정상
        /// </summary>
        T,
        /// <summary>
        /// 비번3번 틀렸을때
        /// </summary>
        H,
        /// <summary>
        /// 사용안함
        /// </summary>
        N,
        /// <summary>
        /// 삭제
        /// </summary>
        D,
        /// <summary>
        /// 비정상
        /// </summary>
        F,
        /// <summary>
        /// 신규입사
        /// </summary>
        X,
    }   

    #region 공용
    public enum ResultMsgStatus : int
    {
        OK = 0,
        ERROR = 1,
        FORMATTER_ERROR = 2,
        INNER_CATCH_ERROR = 3,
        KEY_ERROR = 4,
        ALREADY = 5,
        NOT_AGENT = 6,
        NO_DATA = 7,
        WRONG_PW = 8,
    }

    public enum StatusCode : int
    {
        NULL = -9998,
        FAIL = -9999,
        SUCCESS = 10000,
        UPDATE = 10001,
    }

    public enum DB
    {
        MSSQL_DB_HUB,
        MSSQL_DB_ETC,
       
    }

    

    public enum YN
    {
        Y,
        N,
        D,
    }

    public enum ApiName
    {
        Login,
        token,
    }

    public enum Grp
    {
        [Description("000010")]
        XPVoice,
    }

    #endregion
}

