using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace BlogDataLibrary.Database
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;

        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }

        public List<T> LoadData<T, U>(
            string storedProcedure,
            U parameters,
            string connectionStringName)
        {
            string connectionString =
                _config.GetConnectionString(connectionStringName);

            using (IDbConnection connection =
                new SqlConnection(connectionString))
            {
                return connection.Query<T>(
                    storedProcedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }

        public void SaveData<T>(
            string storedProcedure,
            T parameters,
            string connectionStringName)
        {
            string connectionString =
                _config.GetConnectionString(connectionStringName);

            using (IDbConnection connection =
                new SqlConnection(connectionString))
            {
                connection.Execute(
                    storedProcedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }
    }
}
