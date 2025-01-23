namespace LocationMarker.Entities.ClientModels
{
    public class CountryFromClient
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ISO2 { get; set; } = string.Empty;
        public string ISO3 { get; set; } = string.Empty;
        public string Numeric_Code { get; set; } = string.Empty;
        public string PhoneCode { get; set; } = string.Empty;
        public string Capital { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public string Currency_Name { get; set; } = string.Empty;
        public string Currency_Symbol { get; set; } = string.Empty;
        public string TLD { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public int? Region_Id { get; set; }
        public string SubRegion { get; set; } = string.Empty;
        public int? Subregion_Id { get; set; }
        public string Native { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
        public string Latitude { get; set; } = string.Empty;
        public string Emoji { get; set; } = string.Empty;
        public string EmojiU { get; set; } = string.Empty;
        public string TimeZones { get; set; } = string.Empty;
        public string? Translations { get; set; } = string.Empty;
    }
}
