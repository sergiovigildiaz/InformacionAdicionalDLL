using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformacionAdicional.Persistence
{
    class BaseQuery
    {
        public static DataSet GetDataSet(string sqlCommand, string connectionString)
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sqlCommand, con))
                {
                    con.Open();
                    DataTable table = new DataTable();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        table.Load(dr);
                        ds.Tables.Add(table);
                        dr.Close();
                    }
                }
                con.Close();
            }
            return ds;
        }

        public static void GetDataSetMulti(string connectionString, string query1, string query2)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction();
                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = query1;
                    command.ExecuteNonQuery();
                    command.CommandText = query2;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        throw ex2;
                    }
                }
            }
        }
    }
}
