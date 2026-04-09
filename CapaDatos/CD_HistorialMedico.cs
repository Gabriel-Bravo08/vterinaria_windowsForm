using CapaEntidad;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace CapaDatos
{
    public class CD_HistorialMedico
    {
        private readonly CD_Connection _db = new CD_Connection();

        public List<Cls_HistorialMedico> SelectAll()
        {
            List<Cls_HistorialMedico> lista = new List<Cls_HistorialMedico>();
            using (var cn = _db.GetConnection())
            {
                cn.Open();
                using (var cmd = new SqlCommand("[SQM_MEDICAL].[USP_SelectAll_HistorialMedico]", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Cls_HistorialMedico
                            {
                                HistorialMedicoId = Convert.ToInt32(dr["historialMedicoId"]),
                                CitaId = Convert.ToInt32(dr["citaId"]),
                                Diagnostico = dr["diagnostico"].ToString(),
                                Tratamiento = dr["tratamiento"].ToString(),
                                Observaciones = dr["observaciones"].ToString(),
                                ProximaVisita = dr["proximaVisita"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["proximaVisita"]),
                                EstadoId = Convert.ToInt32(dr["estadoId"]),
                                FechaCreacion = Convert.ToDateTime(dr["fechaCreacion"]),
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

        public string Create(Cls_HistorialMedico obj, int rolId, out int idGenerado)
        {
            idGenerado = 0;
            string mensaje = string.Empty;
            try
            {
                using (var cn = _db.GetConnection())
                {
                    cn.Open();
                    using (var cmd = new SqlCommand("[SQM_MEDICAL].[USP_Create_Historial_Medico]", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EjecutadoPorRolId", rolId);
                        cmd.Parameters.AddWithValue("@citaId", obj.CitaId);
                        cmd.Parameters.AddWithValue("@diagnostico", obj.Diagnostico ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@tratamiento", obj.Tratamiento ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@observaciones", obj.Observaciones ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@proximaVisita", obj.ProximaVisita ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@estadoId", obj.EstadoId);
                        
                        SqlParameter idOut = new SqlParameter("@IdGenerado", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(idOut);

                        cmd.ExecuteNonQuery();
                        idGenerado = (int)idOut.Value;
                        mensaje = "Registro médico creado correctamente.";
                    }
                }
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public bool Update(Cls_HistorialMedico obj, int rolId, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                using (var cn = _db.GetConnection())
                {
                    cn.Open();
                    using (var cmd = new SqlCommand("[SQM_MEDICAL].[USP_Update_Historial_Medico]", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EjecutadoPorRolId", rolId);
                        cmd.Parameters.AddWithValue("@historialMedicoId", obj.HistorialMedicoId);
                        cmd.Parameters.AddWithValue("@citaId", obj.CitaId);
                        cmd.Parameters.AddWithValue("@diagnostico", obj.Diagnostico ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@tratamiento", obj.Tratamiento ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@observaciones", obj.Observaciones ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@proximaVisita", obj.ProximaVisita ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@estadoId", obj.EstadoId);
                        resultado = cmd.ExecuteNonQuery() != 0;
                        if (resultado) mensaje = "Registro médico actualizado correctamente.";
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
                    using (var cmd = new SqlCommand("[SQM_MEDICAL].[USP_Delete_Historial_Medico]", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EjecutadoPorRolId", rolId);
                        cmd.Parameters.AddWithValue("@historialMedicoId", id);
                        resultado = cmd.ExecuteNonQuery() != 0;
                        if (resultado) mensaje = "Registro médico desactivado correctamente.";
                    }
                }
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return resultado;
        }
    }
}
