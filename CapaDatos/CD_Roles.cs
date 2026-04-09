using CapaEntidad;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace CapaDatos
{
    public class CD_Roles
    {
        private readonly CD_Connection _db = new CD_Connection();

        public List<Cls_Roles> SelectAll()
        {
            List<Cls_Roles> lista = new List<Cls_Roles>();
            using (var cn = _db.GetConnection())
            {
                cn.Open();
                using (var cmd = new SqlCommand("[SQM_SECURITY].[USP_SelectAll_Roles]", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Cls_Roles
                            {
                                RolId = Convert.ToInt32(dr["rolId"]),
                                NombreRol = dr["nombreRol"].ToString(),
                                EstadoId = Convert.ToInt32(dr["estadoId"]),
                                NombreEstado = dr["nombreEstado"].ToString()
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public string Create(Cls_Roles obj, out int idGenerado)
        {
            idGenerado = 0;
            string mensaje = string.Empty;
            try
            {
                using (var cn = _db.GetConnection())
                {
                    cn.Open();
                    using (var cmd = new SqlCommand("[SQM_SECURITY].[USP_Create_Roles]", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@nombreRol", obj.NombreRol);
                        cmd.Parameters.AddWithValue("@estadoId", obj.EstadoId);
                        
                        SqlParameter idOut = new SqlParameter("@IdGenerado", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(idOut);

                        cmd.ExecuteNonQuery();
                        idGenerado = (int)idOut.Value;
                        mensaje = "Rol creado correctamente.";
                    }
                }
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public bool Update(Cls_Roles obj, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                using (var cn = _db.GetConnection())
                {
                    cn.Open();
                    using (var cmd = new SqlCommand("[SQM_SECURITY].[USP_Update_Roles]", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@rolId", obj.RolId);
                        cmd.Parameters.AddWithValue("@nombreRol", obj.NombreRol);
                        cmd.Parameters.AddWithValue("@estadoId", obj.EstadoId);
                        resultado = cmd.ExecuteNonQuery() != 0;
                        if (resultado) mensaje = "Rol actualizado correctamente.";
                    }
                }
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return resultado;
        }

        public bool Delete(int id, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                using (var cn = _db.GetConnection())
                {
                    cn.Open();
                    using (var cmd = new SqlCommand("[SQM_SECURITY].[USP_Delete_Roles]", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@rolId", id);
                        resultado = cmd.ExecuteNonQuery() != 0;
                        if (resultado) mensaje = "Rol desactivado correctamente.";
                    }
                }
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return resultado;
        }
    }
}
