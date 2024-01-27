using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ChimLib.Database;
using ChimLib.Utils;

namespace ChimaeraOrderManager
{
    class Program
    {
        static void Main(string[] args)
        {
            List<SqlParameter> p = new List<SqlParameter>();
            DataTable dtOrders = DB.Get("OrderUpdatableSelect", p.ToArray());

            foreach(DataRow drOrder in dtOrders.Rows)
            {
                ScalablePressUtils.UpdateOrderStatus(drOrder["OrderID"].ToString());
            }
        }
    }
}
