using CSharpTypes.Extensions.List;
using Hangfire.Console;
using Hangfire.Server;
using LocationMarker.Data.Interfaces;
using LocationMarker.Entities.Models;
using LocationMarker.Service.Interfaces;
using LocationMarker.Shared.Mapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RedisCache.Common.Repository;
using System.Globalization;

namespace LocationMarker.Service.Implementations
{
    public class ToolServiceV1(IRepositoryManager repository, ILogger<ToolServiceV1> logger, 
        IConfiguration configuration, ICacheCommonRepository redisDb) : IToolServiceV1
    {
        private readonly IRepositoryManager _repository = repository;
        private readonly ILogger<ToolServiceV1> _logger = logger;
        private readonly IConfiguration _configuration = configuration;
        private readonly ICacheCommonRepository _redisDb = redisDb;

        public async Task RunLocationDataLoad(char countriesInitial, PerformContext context)
        {
            var (Countries, Success, Code) = await _repository.ClientLocation.GetCountriesAsync();
            if (Success)
            {
                Countries = Countries
                    .Where(c => c.Name.StartsWith(countriesInitial.ToString(), true, CultureInfo.InvariantCulture))
                    .ToList();

                context.WriteLine($"Iterating on {Countries.Count} countries that starts with {countriesInitial.ToString().ToUpper()}");
                foreach (var countryRecord in Countries.WithProgress(context))
                {
                    var countryDetailResponse = await _repository.ClientLocation.GetCountryAsync(countryRecord.ISO2);
                    if (countryDetailResponse.Success)
                    {
                        var country = ObjectMapper.MapCountry(countryDetailResponse.Country);
                        country.TimeZones = ParseTimeZones(countryDetailResponse.Country.TimeZones);
                        var recordExists = await _repository.Country.RecordExists(x => x.ExtId == country.ExtId);
                        var stateExists = await _repository.State.RecordExists(s => s.CountryExtId == country.ExtId);

                        if (!recordExists || (recordExists && !stateExists))
                        {
                            //Get states for the country
                            var statesResponse = await _repository.ClientLocation.GetStatesAsync(countryRecord.ISO2);
                            if (statesResponse.Success)
                            {
                                if (statesResponse.States.IsNotNullOrEmpty())
                                {
                                    context.WriteLine($"Iterating on {statesResponse.States.Count} {countryRecord.Name} states records");

                                    foreach (var stateRecord in statesResponse.States.WithProgress(context))
                                    {
                                        var stateResponse = await _repository.ClientLocation.GetStateAsync(countryRecord.ISO2, stateRecord.ISO2);
                                        if (stateResponse.Success)
                                        {
                                            context.WriteLine($"Getting details for {stateRecord.Name}, {countryRecord.Name} succeeded.");
                                            var state = ObjectMapper.MapState(country.Id, stateResponse.State);

                                            // Getting cities for the state
                                            var citiesResponse = await _repository.ClientLocation.GetCitiesAsync(countryRecord.ISO2, stateRecord.ISO2);
                                            if (citiesResponse.Success)
                                            {
                                                context.WriteLine($"Got {citiesResponse.Cities.Count} city records for {stateRecord.Name}, {countryRecord.Name}.");
                                                if (citiesResponse.Cities.IsNotNullOrEmpty())
                                                {
                                                    var cities = ObjectMapper.MapCities(state.Id, state.ExtId, country.Id, citiesResponse.Cities);
                                                    var citiesExist = await _repository.City.RecordExists(c => c.StateExtId == state.ExtId);
                                                    if (!stateExists)
                                                    {
                                                        if (!citiesExist)
                                                        {
                                                            context.WriteLine($"Adding {cities.Count} cities for {stateRecord.Name} state, {countryRecord.Name}");
                                                            await _repository.City.AddAsync(cities);
                                                            context.WriteLine($"Added {stateRecord.Name} cities, {stateRecord.Name} to the database");
                                                        }
                                                        else
                                                        {
                                                            context.WriteLine($"City records already exist for {stateRecord.Name}, {countryRecord.Name} in the database");
                                                        }

                                                        context.WriteLine($"Adding {stateRecord.Name} state, {countryRecord.Name}");
                                                        await _repository.State.AddAsync(state);
                                                        context.WriteLine($"Added {stateRecord.Name} states, {countryRecord.Name} to the database");
                                                    }
                                                    else
                                                    {
                                                        context.WriteLine($"State record already exist for {stateRecord.Name}, {countryRecord.Name} in the database");
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                context.WriteLine($"Getting cities for {stateRecord.Name} failed of {countryRecord.Name}. Aborting the whole process!!!");
                                                continue;
                                            }
                                        }
                                        else
                                        {
                                            context.WriteLine($"Getting details for {stateRecord.Name} of {countryRecord.Name} failed. Aborting the whole process!!!");
                                            continue;
                                        }
                                    }
                                }
                                else
                                {
                                    context.WriteLine($"No state record returned for {countryRecord.Name}");
                                }

                                context.WriteLine($"Adding country, {countryRecord.Name} to the database");
                                await _repository.Country.AddAsync(country);
                                context.WriteLine($"Country, {countryRecord.Name} added to the database, together with {statesResponse.States.Count} states");
                            }
                            else
                            {
                                context.WriteLine($"Getting states for {countryRecord.Name} failed. Aborting the whole process!!!");
                                return;
                            }
                        }
                        else
                        {
                            context.WriteLine($"Record exists for {countryRecord.Name}. Skipping!!!");
                            continue;
                        }
                    }
                    else
                    {
                        context.WriteLine($"Http request to get details for {countryRecord.Name} did not succeed. Aborting the whole process!!!");
                        return;
                    }
                }
            }
            else
            {
                context.WriteLine("Http request to get list of countries did not succeed. Operation aborted!!!");
                return;
            }
        }

        private List<LocationTimeZone> ParseTimeZones(string timeZoneString)
        {
            try
            {
                timeZoneString = timeZoneString.Trim().Replace("\\", "");
                var timeZones = JsonConvert.DeserializeObject<List<LocationTimeZone>>(timeZoneString);
                return timeZones ?? [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return [];
            }
        }
    }
}
