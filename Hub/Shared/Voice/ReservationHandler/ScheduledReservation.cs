using Hub.Shared.Interface;
using Hub.Shared.Voice.Request;


namespace Hub.Shared.Voice.ReservationHandler
{
    public class ScheduledReservation : iVoiceReservation
    {
        public int seq { get; set; }

        public DateTime reservationTime { get; set; }
        public ReservationType reservationType { get; set; }

        public string aptCd { get; set; }
        public List<DayOfWeek> dayOfWeeks { get; set; } = new();

        public bool ConflictsWith(iVoiceReservation other)
        {
            if (aptCd != other.aptCd)
                return false;

            if (other is ScheduledReservation scheduledOther)
            {
                return dayOfWeeks.Intersect(scheduledOther.dayOfWeeks).Any() &&
                       Math.Abs((reservationTime - scheduledOther.reservationTime).TotalMinutes) < 3;
            }

            if (other is GeneralReservation generalOther)
            {
                return dayOfWeeks.Contains(generalOther.reservationTime.DayOfWeek) &&
                       Math.Abs((reservationTime - generalOther.reservationTime).TotalMinutes) < 3;
            }

            return false;
        }
    }

}
