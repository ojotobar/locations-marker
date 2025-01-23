using Mongo.Common;

namespace LocationMarker.Entities.Models
{
    public class State : IBaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool IsDeprecated { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
        public long ExtId { get; set; }
        public string Name { get; set; } = string.Empty;
        public long CountryExtId { get; set; }
        public Guid CountryId { get; set; }
        public string CountryCode { get; set; } = string.Empty;
        public string ISO2 { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
        public string Latitude { get; set; } = string.Empty;
        public List<City> Cities { get; set; } = [];
    }
}
