using Mongo.Common;

namespace LocationMarker.Entities.Models
{
    public class ApiKeys : IBaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ApiKey { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
        public bool IsDeprecated { get; set; }
    }
}
