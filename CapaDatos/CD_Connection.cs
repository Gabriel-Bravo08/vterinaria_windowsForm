
using Microsoft.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public class CD_Connection
    {
        private readonly string _connectionString = "Data Source=PC_ADMIN\\SQLENTERPRISE;Initial Catalog=DB_ADCIVETS;Integrated Security=True;Trust Server Certificate=True";

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
        
        public SqlConnection OpenConnection() => GetConnection();
        public void CloseConnection(SqlConnection connection) { if(connection.State == ConnectionState.Open) connection.Close(); }
    }
}
