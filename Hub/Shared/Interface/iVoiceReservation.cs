namespace Hub.Shared.Interface
{
    public interface iVoiceReservation
    {
        int seq { get; set; }
        DateTime reservationTime { get; set; }
        string aptCd { get; set; }
        ReservationType reservationType { get; set; }   
        bool ConflictsWith(iVoiceReservation other);
    }
}
