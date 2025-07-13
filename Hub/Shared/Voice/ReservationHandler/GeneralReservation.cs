using Hub.Shared.Interface;

namespace Hub.Shared.Voice.ReservationHandler
{
    public class GeneralReservation : iVoiceReservation
    {
        public DateTime reservationTime { get; set; }
        public string aptCd { get; set; }
        public int seq { get; set; }
        public ReservationType reservationType { get; set; }

        public bool ConflictsWith(iVoiceReservation other)
        {
            return aptCd == other.aptCd &&
                   Math.Abs((reservationTime - other.reservationTime).TotalMinutes) < 3;
        }
    }
}
