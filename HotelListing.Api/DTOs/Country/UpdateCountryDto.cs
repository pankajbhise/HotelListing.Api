using System.ComponentModel.DataAnnotations;

namespace HotelListing.Api.DTOs.Hotel;

public class UpdateCountryDto : CreateCountryDto
{
    [Required]
    public int Id { get; set; }
}
