using System;
using System.Data;
using System.Data.SqlClient;
using ChimLib.Properties;

namespace ChimLib.Database
{
    public class DB
    {
        private const int SQLTimeout = 30000;

        private static Settings settings = Settings.Default;

        private static readonly string ConnString = settings.ChimaeraArchiveString;

        #region GET
        public static DataTable GetWithQuery(string SQL)
        {
            DataTable dt = new DataTable();
            try
            {
                using(SqlConnection conn = GetConnection())
                {
                    using(SqlCommand cmd = new SqlCommand(SQL, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        dt.Load(cmd.ExecuteReader());
                        return dt;
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable Get(string sProc, SqlParameter[] parms)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(sProc, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        foreach (SqlParameter parm in parms)
                            cmd.Parameters.Add(new SqlParameter(parm.ParameterName, parm.Value));
                        dt.Load(cmd.ExecuteReader(CommandBehavior.Default));
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static SqlConnection GetConnection()
        {
            try
            {
                SqlConnection conn = new SqlConnection(ConnString);
                conn.Open();
                return conn;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region SET
        public static void Set(string sProc, SqlParameter[] parms)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(sProc, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(parms);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SetWithQuery(string SQL)
        {
            try
            {
                using(SqlConnection conn = GetConnection())
                {
                    using(SqlCommand cmd = new SqlCommand(SQL, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int SetWithRowsAffected(string sProc, SqlParameter[] parms)
        {
            int iAffected = 0;
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(sProc, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(parms);
                        iAffected = cmd.ExecuteNonQuery();
                        return iAffected;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}