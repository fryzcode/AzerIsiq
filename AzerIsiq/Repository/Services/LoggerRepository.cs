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
        GROUP BY l.Id, l.EntityName, l.Action, l.EntityId, u.UserName, l.Timestamp
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
    
    // public async Task<IEnumerable<LogEntryDto>> GetFilteredAsync(LogEntryFilterDto filter)
    // {
    //     using var connection = _connectionFactory.CreateConnection();
    //
    //     var sql = @"
    //         SELECT l.Id, l.EntityName AS EntryName, l.EntityId AS EntryId,
    //                u.UserName, u.Role AS UserRole, l.Timestamp
    //         FROM LogEntries l
    //         INNER JOIN Users u ON u.Id = l.UserId
    //         WHERE 1 = 1
    //     ";
    //     
    //     var parameters = new DynamicParameters();
    //
    //     if (!string.IsNullOrEmpty(filter.EntryName))
    //     {
    //         sql += " AND l.EntityName = @EntryName";
    //         parameters.Add("EntryName", filter.EntryName);
    //     }
    //
    //     if (!string.IsNullOrEmpty(filter.UserRole))
    //     {
    //         sql += " AND u.Role = @UserRole";
    //         parameters.Add("UserRole", filter.UserRole);
    //     }
    //
    //     if (!string.IsNullOrEmpty(filter.UserNameSearch))
    //     {
    //         sql += " AND u.UserName LIKE @UserName";
    //         parameters.Add("UserName", $"%{filter.UserNameSearch}%");
    //     }
    //
    //     if (filter.From.HasValue)
    //     {
    //         sql += " AND l.Timestamp >= @From";
    //         parameters.Add("From", filter.From.Value);
    //     }
    //
    //     if (filter.To.HasValue)
    //     {
    //         sql += " AND l.Timestamp <= @To";
    //         parameters.Add("To", filter.To.Value);
    //     }
    //
    //     sql += " ORDER BY l.Timestamp DESC";
    //     sql += " OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
    //
    //     parameters.Add("Offset", (filter.Page - 1) * filter.PageSize);
    //     parameters.Add("PageSize", filter.PageSize);
    //
    //     return await connection.QueryAsync<LogEntryDto>(sql, parameters);
    // }

    // public async Task<int> CountFilteredAsync(LogEntryFilterDto filter)
    // {
    //     using var connection = _connectionFactory.CreateConnection();
    //
    //     var sql = @"
    //         SELECT COUNT(*) 
    //         FROM LogEntries l
    //         INNER JOIN Users u ON u.Id = l.UserId
    //         WHERE 1 = 1
    //     ";
    //
    //     var parameters = new DynamicParameters();
    //
    //     if (!string.IsNullOrEmpty(filter.EntryName))
    //     {
    //         sql += " AND l.EntityName = @EntryName";
    //         parameters.Add("EntryName", filter.EntryName);
    //     }
    //
    //     if (!string.IsNullOrEmpty(filter.UserRole))
    //     {
    //         sql += " AND u.Role = @UserRole";
    //         parameters.Add("UserRole", filter.UserRole);
    //     }
    //
    //     if (!string.IsNullOrEmpty(filter.UserNameSearch))
    //     {
    //         sql += " AND u.UserName LIKE @UserName";
    //         parameters.Add("UserName", $"%{filter.UserNameSearch}%");
    //     }
    //
    //     if (filter.From.HasValue)
    //     {
    //         sql += " AND l.Timestamp >= @From";
    //         parameters.Add("From", filter.From.Value);
    //     }
    //
    //     if (filter.To.HasValue)
    //     {
    //         sql += " AND l.Timestamp <= @To";
    //         parameters.Add("To", filter.To.Value);
    //     }
    //
    //     return await connection.ExecuteScalarAsync<int>(sql, parameters);
    // }
}