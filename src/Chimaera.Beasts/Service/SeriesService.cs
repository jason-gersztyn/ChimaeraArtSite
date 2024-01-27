using Chimaera.Beasts.Model;
using Chimaera.Beasts.Utils;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using System.Data;

namespace Chimaera.Beasts.Service
{
    public static class SeriesService
    {
        public static IEnumerable<Series> GetSeries(int? SeriesID = null, int? GenreID = null)
        {
            using (SqlConnection conn = DB.GetConnection())
                return conn.Query<Series>("GetSeries", new { SeriesID = SeriesID, GenreID = GenreID }, commandType: CommandType.StoredProcedure);
        }

        public static Series CreateSeries(Series series)
        {
            //todo
            List<SqlParameter> Params = new List<SqlParameter>();
            //OTHER FIELDS

            SqlParameter OutSeriesID = new SqlParameter("@OutSeriesID", SqlDbType.Int);
            OutSeriesID.Direction = ParameterDirection.Output;
            Params.Add(OutSeriesID);

            DB.Set("PostSeries", Params.ToArray());

            return GetSeries((int?)OutSeriesID.Value).FirstOrDefault();
        }

        public static Series PutSeries(Series series)
        {
            //todo
            List<SqlParameter> Params = new List<SqlParameter>();
            Params.Add(new SqlParameter("@SeriesID", series.SeriesID));
            //OTHER FIELDS

            DB.Set("PutSeries", Params.ToArray());

            return GetSeries(series.SeriesID).FirstOrDefault();
        }

        public static void DeleteSeries(Series series)
        {
            List<SqlParameter> Params = new List<SqlParameter>();
            Params.Add(new SqlParameter("@SeriesID", series.SeriesID));

            DB.Set("DeleteSeries", Params.ToArray());
        }
    }
}
