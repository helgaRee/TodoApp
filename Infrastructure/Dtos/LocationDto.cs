using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Dtos;

public record LocationDto(int LocationId, string LocationName, string StreetName, string City, string PostalCode);
