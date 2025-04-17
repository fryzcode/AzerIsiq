using AzerIsiq.Data;
using AzerIsiq.Dtos.LogEntryDto;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using Dapper;


namespace AzerIsiq.Repository.Services;

public class LoggerRepository : ILoggerRepository
{
    private readonly AppDbContext _context;
    private readonly IDbConnectionFactory _connectionFactory;

    public LoggerRepository(AppDbContext context, IDbConnectionFactory connectionFactory)
    {
        _context = context;
        _connectionFactory = connectionFactory;
    }

    public async Task LogAsync(LogEntry logEntry)
    {
        await _context.LogEntries.AddAsync(logEntry);
        await _context.SaveChangesAsync();
    }
    
    public async Task<IEnumerable<LogEntryDto>> GetFilteredAsync(LogEntryFilterDto filter)
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = @"
        SELECT 
            l.Id, 
            l.EntityName AS EntryName, 
            l.UserId AS UserId,
            l.Action,
            l.EntityId AS EntryId,
            u.UserName, 
            STRING_AGG(r.RoleName, ', ') AS UserRole, 
            l.Timestamp
        FROM LogEntries l
        INNER JOIN Users u ON u.Id = l.UserId
        INNER JOIN UserRoles ur ON u.Id = ur.UserId
        INNER JOIN Roles r ON ur.RoleId = r.Id
        WHERE 1 = 1
    ";

        var parameters = new DynamicParameters();

        if (!string.IsNullOrEmpty(filter.EntryName))
        {
            sql += " AND l.EntityName = @EntryName";
            parameters.Add("EntryName", filter.EntryName);
        }

        if (!string.IsNullOrEmpty(filter.UserRole))
        {
            sql += " AND r.RoleName = @UserRole";
            parameters.Add("UserRole", filter.UserRole);
        }
        
        if (!string.IsNullOrEmpty(filter.Action))
        {
            sql += " AND l.Action = @Action";
            parameters.Add("Action", filter.Action);
        }

        if (!string.IsNullOrEmpty(filter.UserNameSearch))
        {
            sql += " AND u.UserName LIKE @UserName";
            parameters.Add("UserName", $"%{filter.UserNameSearch}%");
        }

        if (filter.From.HasValue)
        {
            sql += " AND l.Timestamp >= @From";
            parameters.Add("From", filter.From.Value);
        }

        if (filter.To.HasValue)
        {
            sql += " AND l.Timestamp <= @To";
            parameters.Add("To", filter.To.Value);
        }

        sql += @"
        GROUP BY l.Id, l.EntityName, l.Action, l.EntityId, u.UserName, l.Timestamp, l.UserId
        ORDER BY l.Timestamp DESC
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
        ";

        parameters.Add("Offset", (filter.Page - 1) * filter.PageSize);
        parameters.Add("PageSize", filter.PageSize);

        var rawResult = await connection.QueryAsync(sql, parameters);

        var result = rawResult.Select(row => new LogEntryDto
        {
            Id = row.Id,
            EntryName = row.EntryName,
            EntryId = row.EntryId,
            Action = row.Action,
            UserName = row.UserName,
            UserId = row.UserId,
            UserRoles = (row.UserRole as string)?.Split(", ").ToList() ?? new List<string>(),
            Timestamp = TimeZoneInfo.ConvertTimeToUtc(row.Timestamp)
        });

        return result;
    }

    public async Task<int> CountFilteredAsync(LogEntryFilterDto filter)
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = @"
        SELECT COUNT(DISTINCT l.Id) 
        FROM LogEntries l
        INNER JOIN Users u ON u.Id = l.UserId
        INNER JOIN UserRoles ur ON u.Id = ur.UserId
        INNER JOIN Roles r ON ur.RoleId = r.Id
        WHERE 1 = 1
    ";

        var parameters = new DynamicParameters();

        if (!string.IsNullOrEmpty(filter.EntryName))
        {
            sql += " AND l.EntityName = @EntryName";
            parameters.Add("EntryName", filter.EntryName);
        }

        if (!string.IsNullOrEmpty(filter.UserRole))
        {
            sql += " AND r.RoleName = @UserRole";
            parameters.Add("UserRole", filter.UserRole);
        }

        if (!string.IsNullOrEmpty(filter.UserNameSearch))
        {
            sql += " AND u.UserName LIKE @UserName";
            parameters.Add("UserName", $"%{filter.UserNameSearch}%");
        }

        if (filter.From.HasValue)
        {
            sql += " AND l.Timestamp >= @From";
            parameters.Add("From", filter.From.Value);
        }

        if (filter.To.HasValue)
        {
            sql += " AND l.Timestamp <= @To";
            parameters.Add("To", filter.To.Value);
        }

        return await connection.ExecuteScalarAsync<int>(sql, parameters);
    }
    
    public async Task<IEnumerable<string>> GetAllEntityNamesAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = @"SELECT DISTINCT EntityName FROM LogEntries ORDER BY EntityName";
    
        var result = await connection.QueryAsync<string>(sql);
        return result;
    }
    
    public async Task<IEnumerable<LogEntryDto>> GetLogsBySubscriberCodeAsync(string subscriberCode)
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = @"
        SELECT 
            l.Id,
            l.Action,
            l.EntityId AS EntryId,
            l.EntityName AS EntryName,
            l.Timestamp,
            u.Id AS UserId,
            u.UserName,
            STRING_AGG(r.RoleName, ', ') AS UserRole
        FROM LogEntries l
        INNER JOIN Users u ON l.UserId = u.Id
        INNER JOIN UserRoles ur ON u.Id = ur.UserId
        INNER JOIN Roles r ON ur.RoleId = r.Id
        INNER JOIN Subscribers s ON l.EntityId = s.Id
        WHERE l.EntityName = 'Subscriber' AND s.SubscriberCode = @SubscriberCode
        GROUP BY l.Id, l.Action, l.EntityId, l.EntityName, l.Timestamp, u.Id, u.UserName
        ORDER BY l.Timestamp DESC
    ";

        var result = await connection.QueryAsync(sql, new { SubscriberCode = subscriberCode });

        return result.Select(row => new LogEntryDto
        {
            Id = row.Id,
            Action = row.Action,
            EntryId = row.EntryId,
            EntryName = row.EntryName,
            Timestamp = TimeZoneInfo.ConvertTimeToUtc(row.Timestamp),
            UserId = row.UserId,
            UserName = row.UserName,
            UserRoles = (row.UserRole as string)?.Split(", ").ToList() ?? new List<string>()
        });
    }

    

}