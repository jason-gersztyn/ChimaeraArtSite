using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Chimaera.Beasts.Model;
using Chimaera.Beasts.Utils;
using Dapper;

namespace Chimaera.Beasts.Service
{
    public static class ComicService
    {
        public static IEnumerable<Comic> GetComics()
        {
            using (SqlConnection conn = DB.GetConnection())
                return conn.Query<Comic>("GetComics", commandType: CommandType.StoredProcedure);
        }
    }
}