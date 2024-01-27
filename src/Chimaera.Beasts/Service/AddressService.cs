using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Chimaera.Beasts.Model;
using Chimaera.Beasts.Utils;

namespace Chimaera.Beasts.Service
{
    public static class AddressService
    {
        public static Address GetAddress(int QuoteID)
        {
            Address address;

            using (SqlConnection conn = DB.GetConnection())
                address = conn.Query<Address>("GetAddress", new { QuoteID = QuoteID }, commandType: CommandType.StoredProcedure).FirstOrDefault();

            return address;
        }

        public static void CreateAddress(int QuoteID, Address address)
        {
            List<SqlParameter> Params = new List<SqlParameter>();
            Params.Add(new SqlParameter("@QuoteID", QuoteID));
            Params.Add(new SqlParameter("@Email", address.Email));
            Params.Add(new SqlParameter("@Name", address.Name));
            Params.Add(new SqlParameter("@Street1", address.Street1));
            Params.Add(new SqlParameter("@Street2", address.Street2));
            Params.Add(new SqlParameter("@City", address.City));
            Params.Add(new SqlParameter("@State", address.State));
            Params.Add(new SqlParameter("@Country", address.Country));
            Params.Add(new SqlParameter("@Zip", address.Zip));

            DB.Set("PostAddress", Params.ToArray());
        }

        public static void UpdateAddress(Address address)
        {
            List<SqlParameter> Params = new List<SqlParameter>();
            Params.Add(new SqlParameter("@AddressID", address.AddressID));
            Params.Add(new SqlParameter("@Email", address.Email));
            Params.Add(new SqlParameter("@Name", address.Name));
            Params.Add(new SqlParameter("@Street1", address.Street1));
            Params.Add(new SqlParameter("@Street2", address.Street2));
            Params.Add(new SqlParameter("@City", address.City));
            Params.Add(new SqlParameter("@State", address.State));
            Params.Add(new SqlParameter("@Country", address.Country));
            Params.Add(new SqlParameter("@Zip", address.Zip));

            DB.Set("PutAddress", Params.ToArray());
        }

        public static void DeleteAddress(Address address)
        {
            List<SqlParameter> Params = new List<SqlParameter>();
            Params.Add(new SqlParameter("@AddressID", address.AddressID));

            DB.Set("DeleteAddress", Params.ToArray());
        }
    }
}