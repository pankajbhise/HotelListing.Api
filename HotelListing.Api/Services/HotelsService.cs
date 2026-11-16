using HotelListing.Api.Contracts;
using HotelListing.Api.Data;
using HotelListing.Api.DTOs.Hotel;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Api.Services;

public class HotelsService(HotelListingDbContext context) : IHotelsService
{
    public async Task<IEnumerable<GetHotelDto>> GetHotelsAsync()
    {
        // SELECT * FROM Hotels LEFT JOIN Countries ON Hotels.CountryId = Countries.CountryId

        var hotels =  await context.Hotels
            .Include(q  => q.Country)
            .Select(h => new GetHotelDto(
                h.Id,
                h.Name,
                h.Address,
                h.Rating,
                h.CountryId,
                h.Country!.Name
            ))
            .ToListAsync();
        return hotels;
    }

    public async Task<GetHotelDto?> GetHotelAsync(int id)
    {
        // SELECT h.Id
        // h.Name,
        // h.Address,
        // h.Rating,
        // c.Name FROM Hotels
        // LEFT JOIN Countries ON Hotels.CountryId = Countries.CountryId
        // WHERE Hotels.Id = @id

        var Hotel = await context.Hotels
            .Where(q => q.Id == id)
            .Select(h => new GetHotelDto(
                h.Id,
                h.Name,
                h.Address,
                h.Rating,
                h.CountryId,
                h.Country!.Name
            ))
            .FirstOrDefaultAsync();

        return Hotel ?? null;
    }

    public async Task<GetHotelDto> CreateHotelAsync(CreateHotelDto hotelDto)
    {
        var hotel = new Hotel
        {
            Name = hotelDto.Name,
            Address = hotelDto.Address,
            Rating = hotelDto.Rating,
            CountryId = hotelDto.CountryId
        };

        context.Hotels.Add(hotel);
        await context.SaveChangesAsync();
        return new GetHotelDto(
            hotel.Id,
            hotel.Name,
            hotel.Address,
            hotel.Rating,
            hotel.CountryId,
            string.Empty
        );
    }

    public async Task UpdateHotelAsync(int id, UpdateHotelDto updateDto)
    {
        var hotel = await context.Hotels.FindAsync(id) ?? throw new KeyNotFoundException("Hotel not found");
        hotel.Name = updateDto.Name;
        hotel.Address = updateDto.Address;
        hotel.Rating = updateDto.Rating;
        hotel.CountryId = updateDto.CountryId;

        context.Entry(hotel).State = EntityState.Modified;

        context.Hotels.Update(hotel);
        await context.SaveChangesAsync();

    }

    public async Task DeleteHotelAsync(int id)
    {
        var Hotel = await context.Hotels.FindAsync(id) ?? throw new KeyNotFoundException("Hotel not found");

        context.Hotels.Update(Hotel);
        await context.SaveChangesAsync();

    }

    public async Task<bool> HotelExistsAsync(int id)
    {
        return await context.Hotels.AnyAsync(e => e.HotelId == id);
    }

    public async Task<bool> HotelExistsAsync(string name)
    {
        return await context.Hotels.AnyAsync(e => e.Name == name);
    }


}




