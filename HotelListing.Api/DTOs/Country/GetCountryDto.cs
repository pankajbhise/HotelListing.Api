namespace HotelListing.Api.DTOs.Hotel;

public record GetCountryDto(
    int Id,
    string Name,
    string ShortName,
    List<GetHotelSlimDto>? Hotels
);

public record GetCountriesDto(
    int Id,
    string Name,
    string ShortName
);
