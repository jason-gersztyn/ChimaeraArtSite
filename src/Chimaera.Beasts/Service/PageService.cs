using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Chimaera.Beasts.Model;
using Chimaera.Beasts.Utils;
using Dapper;

namespace Chimaera.Beasts.Service
{
    public static class PageService
    {
        public static IEnumerable<Page> GetPages(int ChapterID)
        {
            using (SqlConnection conn = DB.GetConnection())
                return conn.Query<Page>("GetPages", new { ChapterID = ChapterID }, commandType: CommandType.StoredProcedure);
        }
    }
}