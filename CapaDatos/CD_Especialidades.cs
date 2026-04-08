using CapaEntidad;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace CapaDatos
{
    public class CD_Especialidades
    {
        private readonly CD_Connection _db = new CD_Connection();

        public List<Cls_Especialidades> SelectAll()
        {
            List<Cls_Especialidades> lista = new List<Cls_Especialidades>();
            using (var cn = _db.GetConnection())
            {
                cn.Open();
                using (var cmd = new SqlCommand("[SQM_CATALOGS].[USP_SelectAll_Especialidades]", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Cls_Especialidades
                            {
                                EspecialidadId = Convert.ToInt32(dr["especialidadId"]),
                                NombreEspecialidad = dr["nombreEspecialidad"].ToString(),
                                EstadoId = Convert.ToInt32(dr["estadoId"]),
                                NombreEstado = dr["nombreEstado"].ToString()
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public string Create(Cls_Especialidades obj, int rolId, out int idGenerado)
        {
            idGenerado = 0;
            string mensaje = string.Empty;
            try
            {
                using (var cn = _db.GetConnection())
                {
                    cn.Open();
                    using (var cmd = new SqlCommand("[SQM_CATALOGS].[USP_Create_Especialidades]", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EjecutadoPorRolId", rolId);
                        cmd.Parameters.AddWithValue("@nombreEspecialidad", obj.NombreEspecialidad);
                        cmd.Parameters.AddWithValue("@estadoId", obj.EstadoId);
                        
                        SqlParameter idOut = new SqlParameter("@IdGenerado", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(idOut);

                        cmd.ExecuteNonQuery();
                        idGenerado = (int)idOut.Value;
                        mensaje = "Especialidad creada correctamente.";
                    }
                }
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public bool Update(Cls_Especialidades obj, int rolId, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                using (var cn = _db.GetConnection())
                {
                    cn.Open();
                    using (var cmd = new SqlCommand("[SQM_CATALOGS].[USP_Update_Especialidades]", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EjecutadoPorRolId", rolId);
                        cmd.Parameters.AddWithValue("@especialidadId", obj.EspecialidadId);
                        cmd.Parameters.AddWithValue("@nombreEspecialidad", obj.NombreEspecialidad);
                        cmd.Parameters.AddWithValue("@estadoId", obj.EstadoId);
                        resultado = cmd.ExecuteNonQuery() > 0;
                        if (resultado) mensaje = "Especialidad actualizada correctamente.";
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
                    using (var cmd = new SqlCommand("[SQM_CATALOGS].[USP_Delete_Especialidades]", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EjecutadoPorRolId", rolId);
                        cmd.Parameters.AddWithValue("@especialidadId", id);
                        resultado = cmd.ExecuteNonQuery() > 0;
                        if (resultado) mensaje = "Especialidad desactivada correctamente.";
                    }
                }
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return resultado;
        }
    }
}
