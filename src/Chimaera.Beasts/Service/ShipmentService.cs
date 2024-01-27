using Chimaera.Beasts.Model;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using Chimaera.Beasts.Utils;
using Dapper;
using System.Data;

namespace Chimaera.Beasts.Service
{
    public static class ShipmentService
    {
        public static Shipment GetShipment(int OrderID)
        {
            using (SqlConnection conn = DB.GetConnection())
                return conn.Query<Shipment>("GetShipment",
                                            new { OrderID = OrderID },
                                            commandType: CommandType.StoredProcedure).FirstOrDefault();
        }

        public static void CreateShipment(Shipment shipment)
        {
            List<SqlParameter> Params = new List<SqlParameter>();
            Params.Add(new SqlParameter("@OrderID", shipment.OrderID));
            Params.Add(new SqlParameter("@Service", shipment.Service));
            Params.Add(new SqlParameter("@ShipDate", shipment.ShipDate));
            Params.Add(new SqlParameter("@Tracking", shipment.Tracking));

            DB.Set("PostShipment", Params.ToArray());
        }
    }
}
