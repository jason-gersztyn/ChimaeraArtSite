using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Chimaera.Beasts.Model;
using Chimaera.Beasts.Utils;
using Dapper;

namespace Chimaera.Beasts.Service
{
    public static class ChapterService
    {
        public static IEnumerable<Chapter> GetChapters(int ComicID)
        {
            using (SqlConnection conn = DB.GetConnection())
                return conn.Query<Chapter>("GetChapters", new { ComicID = ComicID }, commandType: CommandType.StoredProcedure);
        }
    }
}