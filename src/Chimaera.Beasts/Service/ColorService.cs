using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Chimaera.Beasts.Model;
using Chimaera.Beasts.Utils;
using Dapper;

namespace Chimaera.Beasts.Service
{
    public static class ColorService
    {
        public static Color GetColor(string name)
        {
            Color colorObject = new Color();

            using (SqlConnection conn = DB.GetConnection())
                colorObject = conn.Query<Color>("GetColor",
                     new { Name = name },
                     commandType: CommandType.StoredProcedure).FirstOrDefault();

            return colorObject;
        }

        public static void AddColor(Color color)
        {
            List<SqlParameter> Params = new List<SqlParameter>();

            Params.Add(new SqlParameter("@Name", color.Name));
            Params.Add(new SqlParameter("@PrintAuraID", color.PrintAuraID));

            DB.Set("PostColor", Params.ToArray());
        }

        public static void UpdateColor(Color color)
        {
            List<SqlParameter> Params = new List<SqlParameter>();

            Params.Add(new SqlParameter("@ColorID", color.ColorID));
            Params.Add(new SqlParameter("@PrintAuraID", color.PrintAuraID));

            DB.Set("PutColor", Params.ToArray());
        }
    }
}