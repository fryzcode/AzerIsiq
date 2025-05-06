using AzerIsiq.Data;
using AzerIsiq.Dtos;
using AzerIsiq.Dtos.ElectronicAppealDto;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AzerIsiq.Repository.Services;

public class ElectronicAppealRepository : GenericRepository<ElectronicAppeal>, IElectronicAppealRepository
{
    public ElectronicAppealRepository(AppDbContext context) : base(context)
    {
    }
    
    public async Task<(IEnumerable<ElectronicAppeal> Items, int TotalCount)> GetPagedAsync(PagedRequestDto requestDto, ElectronicAppealFilterDto? filter)
    {
        var query = _context.ElectronicAppeals.AsQueryable();

        if (!string.IsNullOrWhiteSpace(requestDto.Search))
        {
            query = query.Where(x =>
                x.Name.Contains(requestDto.Search) ||
                x.Surname.Contains(requestDto.Search) ||
                x.Email.Contains(requestDto.Search));
        }

        if (filter is not null)
        {
            if (filter.IsRead.HasValue)
                query = query.Where(x => x.IsRead == filter.IsRead.Value);

            if (filter.IsReplied.HasValue)
                query = query.Where(x => x.IsReplied == filter.IsReplied.Value);

            if (filter.Topic.HasValue)
                query = query.Where(x => x.Topic == filter.Topic.Value);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(x => x.Id)
            .Skip((requestDto.Page - 1) * requestDto.PageSize)
            .Take(requestDto.PageSize)
            .ToListAsync();

        return (items, totalCount);
    }

}