namespace LocationMarker.Entities.ClientModels
{
    public class StateFromClient
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public long Country_Id { get; set; }
        public string Country_Code { get; set; } = string.Empty;
        public string ISO2 { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
        public string Latitude { get; set; } = string.Empty;
    }
}
