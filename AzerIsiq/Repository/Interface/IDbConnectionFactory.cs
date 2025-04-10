using System.Data;

namespace AzerIsiq.Repository.Interface;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}