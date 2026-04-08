using CapaEntidad;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace CapaDatos
{
    public class CD_Servicios
    {
        private readonly CD_Connection _db = new CD_Connection();

        public List<Cls_Servicios> SelectAll()
        {
            List<Cls_Servicios> lista = new List<Cls_Servicios>();
            using (var cn = _db.GetConnection())
            {
                cn.Open();
                using (var cmd = new SqlCommand("[SQM_CATALOGS].[USP_SelectAll_Servicios]", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Cls_Servicios
                            {
                                ServicioId = Convert.ToInt32(dr["servicioId"]),
                                NombreServicio = dr["nombreServicio"].ToString(),
                                Descripcion = dr["descripcion"].ToString(),
                                Precio = Convert.ToDecimal(dr["precio"]),
                                EstadoId = Convert.ToInt32(dr["estadoId"]),
                                NombreEstado = dr["nombreEstado"].ToString()
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public string Create(Cls_Servicios obj, int rolId, out int idGenerado)
        {
            idGenerado = 0;
            string mensaje = string.Empty;
            try
            {
                using (var cn = _db.GetConnection())
                {
                    cn.Open();
                    using (var cmd = new SqlCommand("[SQM_CATALOGS].[USP_Create_Servicios]", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EjecutadoPorRolId", rolId);
                        cmd.Parameters.AddWithValue("@nombreServicio", obj.NombreServicio);
                        cmd.Parameters.AddWithValue("@descripcion", obj.Descripcion ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@precio", obj.Precio);
                        cmd.Parameters.AddWithValue("@estadoId", obj.EstadoId);

                        SqlParameter idOut = new SqlParameter("@IdGenerado", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(idOut);

                        cmd.ExecuteNonQuery();
                        idGenerado = (int)idOut.Value;
                        mensaje = "Servicio creado correctamente.";
                    }
                }
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public bool Update(Cls_Servicios obj, int rolId, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                using (var cn = _db.GetConnection())
                {
                    cn.Open();
                    using (var cmd = new SqlCommand("[SQM_CATALOGS].[USP_Update_Servicios]", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EjecutadoPorRolId", rolId);
                        cmd.Parameters.AddWithValue("@servicioId", obj.ServicioId);
                        cmd.Parameters.AddWithValue("@nombreServicio", obj.NombreServicio);
                        cmd.Parameters.AddWithValue("@descripcion", obj.Descripcion ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@precio", obj.Precio);
                        cmd.Parameters.AddWithValue("@estadoId", obj.EstadoId);
                        resultado = cmd.ExecuteNonQuery() > 0;
                        if (resultado) mensaje = "Servicio actualizado correctamente.";
                    }
                }
            }
            catch (Exception ex) { mensaje = ex.Message; }
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
                    using (var cmd = new SqlCommand("[SQM_CATALOGS].[USP_Delete_Servicios]", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EjecutadoPorRolId", rolId);
                        cmd.Parameters.AddWithValue("@servicioId", id);
                        resultado = cmd.ExecuteNonQuery() > 0;
                        if (resultado) mensaje = "Servicio desactivado correctamente.";
                    }
                }
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return resultado;
        }
    }
}
