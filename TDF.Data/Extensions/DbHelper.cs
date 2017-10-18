using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using TDF.Core.Configuration;

namespace TDF.Data.Extensions
{
    public class DbHelper
    {
        private static readonly string Connstring = Configs.Instance.ConnectionString;

        public static int ExecuteSqlCommand(string cmdText)
        {
            using (DbConnection conn = new SqlConnection(Connstring))
            {
                DbCommand cmd = new SqlCommand();
                PrepareCommand(cmd, conn, null, CommandType.Text, cmdText, null);
                return cmd.ExecuteNonQuery();
            }
        }

        private static void PrepareCommand(DbCommand cmd, DbConnection conn, DbTransaction trans, CommandType cmdType, string cmdText, DbParameter[] cmdpParams)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = cmdType;
            if (cmdpParams != null)
            {
                cmd.Parameters.AddRange(cmdpParams);
            }
        }
    }
}
