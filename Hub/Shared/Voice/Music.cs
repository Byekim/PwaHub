namespace Hub.Shared.Voice
{
    public class Music
    {
        public bool check { get; set; }
        public string? fileName { get; set; }
        public string? fullPath { get; set; }
    }

    public class MusicSetting
    {
        public int seq { get; set; }
        public List<DayOfWeek>? dayOfWeeks { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public List<Music>? musicPlayList { get; set; }
    }
}
