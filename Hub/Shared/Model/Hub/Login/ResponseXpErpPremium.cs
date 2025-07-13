using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hub.Shared.Model.Hub.Login
{
    public class ResponseXpErpPremium
    {
        public string xpElectronicPaymentYn { get; set; } // 전자결재
        public string firefightingFacilitiesYn { get; set; } // 소방시설
        public string visitingVehicleYn { get; set; } //방문차량
        public string xpDocumentBoxYn { get; set; } // 문서함
        public string transYn { get; set; } // 전환여부
        public string userName { get; set; }      //사용자
    }
}
