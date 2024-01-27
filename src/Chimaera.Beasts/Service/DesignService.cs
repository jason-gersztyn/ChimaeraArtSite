using Chimaera.Beasts.Model;
using Chimaera.Beasts.Utils;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace Chimaera.Beasts.Service
{
    public static class DesignService
    {
        public static IEnumerable<Design> GetDesigns(int? DesignID = null)
        {
            using (SqlConnection conn = DB.GetConnection())
                return conn.Query<Design>("GetDesigns", new { DesignID = DesignID }, commandType: CommandType.StoredProcedure);
        }

        public static Design CreateDesign(Series series, Design design)
        {
            List<SqlParameter> Params = new List<SqlParameter>();
            Params.Add(new SqlParameter("@Name", design.Name));
            Params.Add(new SqlParameter("@SeriesID", series.SeriesID));
            Params.Add(new SqlParameter("@Description", design.Description));
            Params.Add(new SqlParameter("@Active", design.Active));

            SqlParameter OutDesignID = new SqlParameter("@OutDesignID", SqlDbType.Int);
            OutDesignID.Direction = ParameterDirection.Output;
            Params.Add(OutDesignID);

            DB.Set("PostDesign", Params.ToArray());

            return GetDesigns((int?)OutDesignID.Value).FirstOrDefault();
        }

        public static Design UpdateDesign(Series series, Design design)
        {
            List<SqlParameter> Params = new List<SqlParameter>();
            Params.Add(new SqlParameter("@DesignID", design.DesignID));
            Params.Add(new SqlParameter("@Name", design.Name));
            Params.Add(new SqlParameter("@SeriesID", series.SeriesID));
            Params.Add(new SqlParameter("@Description", design.Description));
            Params.Add(new SqlParameter("@Active", design.Active));

            DB.Set("PutDesign", Params.ToArray());

            return GetDesigns(design.DesignID).FirstOrDefault();
        }

        public static void DeleteDesign(Design design)
        {
            List<SqlParameter> Params = new List<SqlParameter>();
            Params.Add(new SqlParameter("@DesignID", design.DesignID));
            DB.Set("DeleteDesign", Params.ToArray());
        }
    }
}
