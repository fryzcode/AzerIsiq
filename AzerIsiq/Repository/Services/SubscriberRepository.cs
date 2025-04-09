using AzerIsiq.Data;
using AzerIsiq.Dtos;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AzerIsiq.Repository.Services;

public class SubscriberRepository : GenericRepository<Subscriber>, ISubscriberRepository
{
    public SubscriberRepository(AppDbContext context) : base(context)
    {
        
    }
    
    public async Task<PagedResultDto<SubscriberDto>> GetSubscriberByFiltersAsync(SubscriberFilterDto dto)
    {
        var query = _context.Subscribers
            .Include(s => s.Region)
            .Include(s => s.District)
            .Include(s => s.Territory)
            .Include(s => s.Street)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(dto.Name))
            query = query.Where(s => s.Name.Contains(dto.Name));

        if (!string.IsNullOrWhiteSpace(dto.Surname))
            query = query.Where(s => s.Surname.Contains(dto.Surname));

        if (!string.IsNullOrWhiteSpace(dto.Patronymic))
            query = query.Where(s => s.Patronymic.Contains(dto.Patronymic));

        if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
            query = query.Where(s => s.PhoneNumber.Contains(dto.PhoneNumber));

        if (!string.IsNullOrWhiteSpace(dto.FinCode))
            query = query.Where(s => s.FinCode.Contains(dto.FinCode));

        if (dto.PopulationStatus.HasValue)
            query = query.Where(s => s.PopulationStatus == dto.PopulationStatus.Value);

        if (dto.RegionId.HasValue)
            query = query.Where(s => s.RegionId == dto.RegionId);

        if (!string.IsNullOrWhiteSpace(dto.RegionName))
            query = query.Where(s => s.Region != null && s.Region.Name.Contains(dto.RegionName));

        if (dto.DistrictId.HasValue)
            query = query.Where(s => s.DistrictId == dto.DistrictId);

        if (!string.IsNullOrWhiteSpace(dto.DistrictName))
            query = query.Where(s => s.District != null && s.District.Name.Contains(dto.DistrictName));

        if (dto.TerritoryId.HasValue)
            query = query.Where(s => s.TerritoryId == dto.TerritoryId);

        if (!string.IsNullOrWhiteSpace(dto.TerritoryName))
            query = query.Where(s => s.Territory != null && s.Territory.Name.Contains(dto.TerritoryName));

        if (dto.StreetId.HasValue)
            query = query.Where(s => s.StreetId == dto.StreetId);

        if (!string.IsNullOrWhiteSpace(dto.StreetName))
            query = query.Where(s => s.Street != null && s.Street.Name.Contains(dto.StreetName));

        if (!string.IsNullOrWhiteSpace(dto.Building))
            query = query.Where(s => s.Building.Contains(dto.Building));

        if (!string.IsNullOrWhiteSpace(dto.Apartment))
            query = query.Where(s => s.Apartment.Contains(dto.Apartment));

        if (dto.Status.HasValue)
            query = query.Where(s => s.Status == dto.Status);

        if (!string.IsNullOrWhiteSpace(dto.Ats))
            query = query.Where(s => s.Ats.Contains(dto.Ats));

        if (!string.IsNullOrWhiteSpace(dto.SubscriberCode))
            query = query.Where(s => s.SubscriberCode.Contains(dto.SubscriberCode));

        if (dto.CreatedDate.HasValue)
            query = query.Where(s => s.CreatedAt.Date == dto.CreatedDate.Value.Date);
        
        if (!dto.CreatedDate.HasValue)
            query = query.OrderByDescending(s => s.CreatedAt);

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((dto.Page - 1) * dto.PageSize)
            .Take(dto.PageSize)
            .Select(s => new SubscriberDto
            {
                Id = s.Id,
                Name = s.Name,
                Surname = s.Surname,
                Patronymic = s.Patronymic,
                PhoneNumber = s.PhoneNumber,
                FinCode = s.FinCode,
                PopulationStatus = s.PopulationStatus,
                RegionId = s.RegionId,
                RegionName = s.Region != null ? s.Region.Name : null,
                DistrictId = s.DistrictId,
                DistrictName = s.District != null ? s.District.Name : null,
                TerritoryId = s.TerritoryId,
                TerritoryName = s.Territory != null ? s.Territory.Name : null,
                StreetId = s.StreetId,
                StreetName = s.Street != null ? s.Street.Name : null,
                Building = s.Building,
                Apartment = s.Apartment,
                Status = s.Status,
                Ats = s.Ats,
                SubscriberCode = s.SubscriberCode,
                CreatedAt = s.CreatedAt
            })
            .ToListAsync();

        return new PagedResultDto<SubscriberDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = dto.Page,
            PageSize = dto.PageSize
        };
    }
    public async Task<string> GenerateUniqueAtsAsync()
    {
        var random = new Random();
        string atsCode;
        bool exists;

        do
        {
            atsCode = "ATS" + string.Concat(Enumerable.Range(0, 15).Select(_ => random.Next(0, 10)));
            exists = await _context.Set<Subscriber>().AnyAsync(s => s.Ats == atsCode);
        }
        while (exists);

        return atsCode;
    }
    public async Task<bool> ExistsBySubscriberCodeAsync(string subscriberCode)
    {
        return await _context.Subscribers.AnyAsync(s => s.SubscriberCode == subscriberCode);
    }
    public async Task<bool> ExistsBySubscriberFinAsync(string finCode)
    {
        return await _context.Subscribers.AnyAsync(s => s.FinCode == finCode);
    }
}