using CapaEntidad;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace CapaDatos
{
    public class CD_Citas
    {
        private readonly CD_Connection _db = new CD_Connection();

        public List<Cls_Citas> SelectAll()
        {
            List<Cls_Citas> lista = new List<Cls_Citas>();
            using (var cn = _db.GetConnection())
            {
                cn.Open();
                using (var cmd = new SqlCommand("[SQM_APPOINTMENTS].[USP_SelectAll_Citas]", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Cls_Citas
                            {
                                CitaId = Convert.ToInt32(dr["citaId"]),
                                MascotaId = Convert.ToInt32(dr["mascotaId"]),
                                VeterinarioId = Convert.ToInt32(dr["veterinarioId"]),
                                FechaCita = Convert.ToDateTime(dr["fechaCita"]),
                                Notas = dr["notas"].ToString(),
                                EstadoId = Convert.ToInt32(dr["estadoId"]),
                                NombreMascota = dr["nombreMascota"].ToString(),
                                NombreVeterinario = dr["nombreVeterinario"].ToString(),
                                NombreEstado = dr["nombreEstado"].ToString()
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public string Create(Cls_Citas obj, int rolId, out int idGenerado)
        {
            idGenerado = 0;
            string mensaje = string.Empty;
            try
            {
                using (var cn = _db.GetConnection())
                {
                    cn.Open();
                    using (var cmd = new SqlCommand("[SQM_APPOINTMENTS].[USP_Create_Citas]", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EjecutadoPorRolId", rolId);
                        cmd.Parameters.AddWithValue("@mascotaId", obj.MascotaId);
                        cmd.Parameters.AddWithValue("@veterinarioId", obj.VeterinarioId);
                        cmd.Parameters.AddWithValue("@creadoPor", obj.CreadoPor ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@fechaCita", obj.FechaCita);
                        cmd.Parameters.AddWithValue("@notas", obj.Notas ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@estadoId", obj.EstadoId);
                        
                        SqlParameter idOut = new SqlParameter("@IdGenerado", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(idOut);

                        cmd.ExecuteNonQuery();
                        idGenerado = (int)idOut.Value;
                        mensaje = "Cita creada correctamente.";
                    }
                }
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public bool Update(Cls_Citas obj, int rolId, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                using (var cn = _db.GetConnection())
                {
                    cn.Open();
                    using (var cmd = new SqlCommand("[SQM_APPOINTMENTS].[USP_Update_Citas]", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EjecutadoPorRolId", rolId);
                        cmd.Parameters.AddWithValue("@citaId", obj.CitaId);
                        cmd.Parameters.AddWithValue("@mascotaId", obj.MascotaId);
                        cmd.Parameters.AddWithValue("@veterinarioId", obj.VeterinarioId);
                        cmd.Parameters.AddWithValue("@fechaCita", obj.FechaCita);
                        cmd.Parameters.AddWithValue("@notas", obj.Notas ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@estadoId", obj.EstadoId);
                        resultado = cmd.ExecuteNonQuery() != 0;
                        if (resultado) mensaje = "Cita actualizada correctamente.";
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
                    using (var cmd = new SqlCommand("[SQM_APPOINTMENTS].[USP_Delete_Citas]", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EjecutadoPorRolId", rolId);
                        cmd.Parameters.AddWithValue("@citaId", id);
                        resultado = cmd.ExecuteNonQuery() != 0;
                        if (resultado) mensaje = "Cita cancelada correctamente.";
                    }
                }
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return resultado;
        }
    }
}
