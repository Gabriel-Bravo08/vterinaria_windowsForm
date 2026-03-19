using System;
using System.Data;
using CapaEntidad;
using Microsoft.Data.SqlClient;

namespace CapaDatos
{
    public class CD_Login
    {
        private readonly CD_Connection _db = new CD_Connection();

        public Cls_Personal ValidarLogin(string usuario, string clave)
        {
            Cls_Personal personal = null;
            using (var connection = _db.GetConnection())
            {
                connection.Open();
                using (var cmd = new SqlCommand("[SQM_SECURITY].[USP_Login]", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Usuario", usuario);
                    cmd.Parameters.AddWithValue("@Clave", clave);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            personal = new Cls_Personal
                            {
                                PersonalId = Convert.ToInt32(reader["personalId"]),
                                PrimerNombre = reader["primerNombre"].ToString(),
                                PrimerApellido = reader["primerApellido"].ToString(),
                                NombreUsuario = reader["nombreUsuario"].ToString(),
                                RolId = Convert.ToInt32(reader["rolId"]),
                                EstadoId = Convert.ToInt32(reader["estadoId"])
                            };
                        }
                    }
                }
            }
            return personal;
        }
    }
}
