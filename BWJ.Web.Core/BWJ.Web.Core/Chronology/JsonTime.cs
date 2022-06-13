namespace BWJ.Web.Core.Chronology
{
    public class JsonTime
    {
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
        public int? UtcHourOffset { get; set; }
        public int? UtcMinuteOffset { get; set; }
        public int? UtcSecondOffset { get; set; }
    }
}
