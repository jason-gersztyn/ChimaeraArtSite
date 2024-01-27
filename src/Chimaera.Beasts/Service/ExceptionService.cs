using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Chimaera.Beasts.Utils;

namespace Chimaera.Beasts.Service
{
    public static class ExceptionService
    {
        public static void CreateException(Exception ex)
        {
            List<SqlParameter> p = new List<SqlParameter>();
            p.Add(new SqlParameter("@Message", ex.Message));
            p.Add(new SqlParameter("@ErrorType", ex.GetType().Name));
            p.Add(new SqlParameter("@Stacktrace", ex.StackTrace));
            DB.Set("PostException", p.ToArray());
        }
    }
}