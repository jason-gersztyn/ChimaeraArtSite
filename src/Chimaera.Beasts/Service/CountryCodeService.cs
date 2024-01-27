using Chimaera.Beasts.Model;
using Chimaera.Beasts.Utils;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Chimaera.Beasts.Service
{
    public static class CountryCodeService
    {
        public static IEnumerable<CountryCode> GetCountries()
        {
            using (SqlConnection conn = DB.GetConnection())
                return conn.Query<CountryCode>("GetCountries", commandType: CommandType.StoredProcedure);
        }
    }
}
