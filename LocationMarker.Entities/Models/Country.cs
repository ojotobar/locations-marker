using Mongo.Common;

namespace LocationMarker.Entities.Models
{
    public class Country : IBaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool IsDeprecated { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
        public long ExtId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ISO2 { get; set; } = string.Empty;
        public string ISO3 { get; set; } = string.Empty;
        public string NumericCode { get; set; } = string.Empty;
        public string PhoneCode { get; set; } = string.Empty;
        public string Capital { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public string CurrencyName { get; set; } = string.Empty;
        public string CurrencySymbol { get; set; } = string.Empty;
        public string TLD { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public int? RegionId { get; set; }
        public string SubRegion { get; set; } = string.Empty;
        public int? SubRegionId { get; set; }
        public string Native { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
        public string Latitude { get; set; } = string.Empty;
        public string Emoji { get; set; } = string.Empty;
        public string EmojiUnicode { get; set; } = string.Empty;
        public List<LocationTimeZone> TimeZones { get; set; } = [];
        public List<State> States { get; set; } = [];
    }
}
