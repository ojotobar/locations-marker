namespace LocationMarker.Entities.ClientModels
{
    public class CityFromClient
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
        public string Latitude { get; set; } = string.Empty;
    }
}
