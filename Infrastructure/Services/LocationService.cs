using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Infrastructure.Services;

public class LocationService(LocationRepository locationRepository)
{
    private readonly LocationRepository _locationRepository = locationRepository;


    //CREATE
    public async Task<LocationEntity> CreateLocationAsync(LocationEntity entity)
    {
        try
        {
            //skapa location om den inte redan finns
            var locationEntity = _locationRepository.GetAsync(x => x.LocationId == entity.LocationId);
            if (locationEntity == null)
            {
                //skapa ny location och mappa in egenskaper
                var newLocation = await _locationRepository.CreateAsync(new LocationEntity
                {
                    LocationId = entity.LocationId,
                    LocationName = entity.LocationName,
                    StreetName = entity.StreetName,
                    PostalCode = entity.PostalCode,
                    City = entity.City,              
                });
                return newLocation;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
  
    }

    //GET ONE
    public async Task<bool> GetLocation(Expression<Func<LocationEntity, bool>>expression)
    {
        try
        {
            var locationEntity = await _locationRepository.GetAsync(expression);
            if (locationEntity != null)
            {
                return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return false;
    }

    //GETALL
    public async Task<IEnumerable<LocationEntity>> GetLocations()
    {
        try
        {
            var locationEntities = await _locationRepository.GetAllAsync();
            if (locationEntities != null)
            {
                return locationEntities;
            }

        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    //UPDATE
    public async Task<bool> UpdateLocation(LocationDto dto)
    {
        try
        {
            //skapa ny instans av entiteten, skapa en kopia med de uppdaterade uppgifterna
            var updatedLocation = new LocationEntity
            {
                LocationId = dto.LocationId,
                LocationName = dto.LocationName,
                StreetName = dto.StreetName,
                City = dto.City,
                PostalCode = dto.PostalCode,
            };

            //spara in de uppdaterade uppgifterna till den nya Locationen, uppdatera!
            var newLocation = await _locationRepository.UpdateAsync(x => x.LocationId == dto.LocationId, updatedLocation);
            //mappa in
            if (newLocation != null)
            {
                var locationDto = new LocationDto
                    (
                        newLocation.LocationId,
                        newLocation.LocationName!,
                        newLocation.StreetName!,
                        newLocation.City!,
                        newLocation.PostalCode!           
                    );
                return locationDto != null;                          
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return false;
    }


    public async Task<bool> DeleteLocation(Expression<Func<LocationEntity, bool>> expression)
    {
        try
        {
            var result = await _locationRepository.DeleteAsync(expression);
                return result;     
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return false;   
    }
}
