using Hub.Shared.Interface;

namespace Hub.Shared.Voice.ReservationHandler
{
    public class PeriodReservation : iVoiceReservation
    {
        public int seq { get; set; }

        public DateTime reservationTime { get; set; }
        public string aptCd { get; set; }
        public ReservationType reservationType { get; set; }

        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

        public bool ConflictsWith(iVoiceReservation other)
        {
            if (aptCd != other.aptCd)
                return false;

            if (other is PeriodReservation periodOther)
            {
                return startDate <= periodOther.endDate &&
                       endDate >= periodOther.startDate &&
                       Math.Abs((reservationTime - periodOther.reservationTime).TotalMinutes) < 3;
            }

            if (other is GeneralReservation generalOther)
            {
                return generalOther.reservationTime >= startDate &&
                       generalOther.reservationTime <= endDate &&
                       Math.Abs((reservationTime - generalOther.reservationTime).TotalMinutes) < 3;
            }

            return false;
        }
    }
}
