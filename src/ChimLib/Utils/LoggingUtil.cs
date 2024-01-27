using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChimLib.Database;

namespace ChimLib.Utils
{
    public static class LoggingUtil
    {
        public static void InsertError(Exception ex)
        {
            /*string logFilePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Log";
            string logFileName = "ChimaeraErrorLog.log";

            if (!Directory.Exists(logFilePath))
                Directory.CreateDirectory(logFilePath);

            string logFile = Path.Combine(logFilePath, logFileName);

            using(FileStream fs = File.OpenWrite(logFile))
            {
                byte[] theBytes = ReadAllBytes(ex.ToString());
                fs.Write(theBytes, 0, theBytes.Length);
            }*/

            List<SqlParameter> p = new List<SqlParameter>();
            p.Add(new SqlParameter("@Message", ex.Message));
            p.Add(new SqlParameter("@ErrorType", ex.GetType().Name));
            p.Add(new SqlParameter("@Stacktrace", ex.StackTrace));
            DB.Set("ExceptionsInsertNew", p.ToArray());
        }

        public static byte[] ReadAllBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}