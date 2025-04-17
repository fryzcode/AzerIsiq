// Repository/Services/SqlConnectionFactory.cs
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using AzerIsiq.Repository.Interface;
using Microsoft.Data.SqlClient;

public class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly IConfiguration _configuration;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_configuration.GetConnectionString("DBConnection"));
    }
}