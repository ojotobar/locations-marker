namespace LocationMarker.Entities.Models
{
    public class LocationTimeZone
    {
        public string ZoneName { get; set; } = string.Empty;
        public int GMTOffset { get; set; }
        public string GMTOffsetName { get; set; } = string.Empty;
        public string Abbreviation { get; set; } = string.Empty;
        public string TZName { get; set; } = string.Empty;

    }
}
