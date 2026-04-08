using CapaEntidad;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Clientes
    {
        private readonly CD_Connection _db = new CD_Connection();

        public List<Cls_Clientes> SelectAll()
        {
            List<Cls_Clientes> lista = new List<Cls_Clientes>();
            using (var cn = _db.GetConnection())
            {
                cn.Open();
                using (var cmd = new SqlCommand("[SQM_CORE].[USP_SelectAll_Clientes]", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Cls_Clientes
                            {
                                ClienteId = Convert.ToInt32(dr["clienteId"]),
                                Nombre = dr["nombre"].ToString(),
                                Apellido = dr["apellido"].ToString(),
                                Telefono = dr["telefono"].ToString(),
                                Email = dr["email"].ToString(),
                                Direccion = dr["direccion"].ToString(),
                                EstadoId = Convert.ToInt32(dr["estadoId"]),
                                NombreEstado = dr["nombreEstado"].ToString(),
                                FechaCreacion = Convert.ToDateTime(dr["fechaCreacion"])
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public string Create(Cls_Clientes obj, int rolId, out int idGenerado)
        {
            idGenerado = 0;
            string mensaje = string.Empty;
            try
            {
                using (var cn = _db.GetConnection())
                {
                    cn.Open();
                    using (var cmd = new SqlCommand("[SQM_CORE].[USP_Create_Clientes]", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EjecutadoPorRolId", rolId);
                        cmd.Parameters.AddWithValue("@nombre", obj.Nombre);
                        cmd.Parameters.AddWithValue("@apellido", obj.Apellido);
                        cmd.Parameters.AddWithValue("@telefono", obj.Telefono ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@email", obj.Email ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@direccion", obj.Direccion ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@estadoId", obj.EstadoId);
                        
                        SqlParameter idOut = new SqlParameter("@IdGenerado", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(idOut);

                        cmd.ExecuteNonQuery();
                        idGenerado = (int)idOut.Value;
                        mensaje = "Cliente creado exitosamente.";
                    }
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return mensaje;
        }

        public bool Update(Cls_Clientes obj, int rolId, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                using (var cn = _db.GetConnection())
                {
                    cn.Open();
                    using (var cmd = new SqlCommand("[SQM_CORE].[USP_Update_Clientes]", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EjecutadoPorRolId", rolId);
                        cmd.Parameters.AddWithValue("@clienteId", obj.ClienteId);
                        cmd.Parameters.AddWithValue("@nombre", obj.Nombre);
                        cmd.Parameters.AddWithValue("@apellido", obj.Apellido);
                        cmd.Parameters.AddWithValue("@telefono", obj.Telefono ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@email", obj.Email ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@direccion", obj.Direccion ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@estadoId", obj.EstadoId);

                        resultado = cmd.ExecuteNonQuery() > 0;
                        if (resultado) mensaje = "Cliente actualizado exitosamente.";
                    }
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }
            return resultado;
        }

        public bool Delete(int id, int rolId, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                using (var cn = _db.GetConnection())
                {
                    cn.Open();
                    using (var cmd = new SqlCommand("[SQM_CORE].[USP_Delete_Clientes]", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EjecutadoPorRolId", rolId);
                        cmd.Parameters.AddWithValue("@clienteId", id);
                        resultado = cmd.ExecuteNonQuery() > 0;
                        if (resultado) mensaje = "Cliente desactivado exitosamente.";
                    }
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }
            return resultado;
        }
    }
}
