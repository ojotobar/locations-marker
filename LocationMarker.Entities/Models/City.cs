using Mongo.Common;

namespace LocationMarker.Entities.Models
{
    public class City : IBaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool IsDeprecated { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
        public Guid StateId { get; set; }
        public Guid CountryId { get; set; }
        public long ExtId { get; set; }
        public long StateExtId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
        public string Latitude { get; set; } = string.Empty;
    }
}
