using LocationMarker.Entities.ClientModels;
using LocationMarker.Entities.Models;

namespace LocationMarker.Shared.Mapper
{
    public class ObjectMapper
    {
        public static Country MapCountry(CountryFromClient client)
        {
            return client != null ? new Country
            {
                ExtId = client.Id,
                Capital = client.Capital,
                Currency = client.Currency,
                CurrencyName = client.Currency_Name,
                CurrencySymbol = client.Currency_Symbol,
                Emoji = client.Emoji,
                EmojiUnicode = client.EmojiU,
                ISO2 = client.ISO2,
                ISO3 = client.ISO3,
                Latitude = client.Latitude,
                Longitude = client.Longitude,
                Name = client.Name,
                Nationality = client.Nationality,
                Native = client.Native,
                NumericCode = client.Numeric_Code,
                PhoneCode = client.PhoneCode,
                Region = client.Region,
                RegionId = client.Region_Id,
                SubRegion = client.SubRegion,
                SubRegionId = client.Subregion_Id,
                TLD = client.TLD,
            } : new();
        }

        public static State MapState(Guid countryId, StateFromClient client)
        {
            return client != null ? new State
            {
                ExtId = client.Id,
                CountryCode = client.Country_Code,
                CountryExtId = client.Country_Id,
                ISO2 = client.ISO2,
                Latitude = client.Latitude,
                Longitude = client.Longitude,
                Name = client.Name,
                Type = client.Type,
                CountryId = countryId,
            } : new();
        }

        public static City MapCity(Guid stateId, CityFromClient client)
        {
            return client != null ? new City
            {
                ExtId = client.Id,
                Latitude = client.Latitude,
                Longitude = client.Longitude,
                Name = client.Name,
                StateExtId = client.Id,
                StateId = stateId
            } : new();
        }

        public static List<City> MapCities(Guid stateId, long stateExtId, Guid countryId, List<CityFromClient> cities)
        {
            var result = new List<City>();
            foreach (var city in cities)
            {
                result.Add(new City
                {
                    ExtId = city.Id,
                    Latitude = city.Latitude,
                    Longitude = city.Longitude,
                    Name = city.Name,
                    StateExtId = stateExtId,
                    StateId = stateId,
                    CountryId = countryId
                });
            }
            return result;
        }
    }
}
