using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace RestaurantAPI.Repositories
{
    public class RestaurantDapperRepository
    {
        private readonly IDbConnection _connection;

        public RestaurantDapperRepository(IDbConnection dbConnection)
        {
            _connection = dbConnection;
        }

        public IEnumerable<dynamic> SearchVulnearable(string searchPhrase)
        {
            string sql = $"SELECT * FROM \"Restaurants\" WHERE \"Name\" = '{searchPhrase}'";
             return _connection.Query(sql);
        }

        public IEnumerable<dynamic> SearchSafe(string searchPhrase)
        {
            string sql = "SELECT * FROM \"Restaurants\" WHERE \"Name\" = @Search";
            return _connection.Query(sql, new { Search = searchPhrase});
        }

    }
}