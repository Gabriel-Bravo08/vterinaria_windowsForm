using CapaEntidad;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace CapaDatos
{
    public class CD_Especies
    {
        private readonly CD_Connection _db = new CD_Connection();

        public List<Cls_Especies> SelectAll()
        {
            List<Cls_Especies> lista = new List<Cls_Especies>();
            using (var cn = _db.GetConnection())
            {
                cn.Open();
                using (var cmd = new SqlCommand("[SQM_CATALOGS].[USP_SelectAll_Especies]", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Cls_Especies
                            {
                                EspecieId = Convert.ToInt32(dr["especieId"]),
                                NombreEspecie = dr["nombreEspecie"].ToString(),
                                EstadoId = Convert.ToInt32(dr["estadoId"]),
                                NombreEstado = dr["nombreEstado"].ToString()
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public string Create(Cls_Especies obj, int rolId, out int idGenerado)
        {
            idGenerado = 0;
            string mensaje = string.Empty;
            try
            {
                using (var cn = _db.GetConnection())
                {
                    cn.Open();
                    using (var cmd = new SqlCommand("[SQM_CATALOGS].[USP_Create_Especies]", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EjecutadoPorRolId", rolId);
                        cmd.Parameters.AddWithValue("@nombreEspecie", obj.NombreEspecie);
                        cmd.Parameters.AddWithValue("@estadoId", obj.EstadoId);
                        
                        SqlParameter idOut = new SqlParameter("@IdGenerado", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(idOut);

                        cmd.ExecuteNonQuery();
                        
                        idGenerado = (int)idOut.Value;
                        mensaje = "Especie creada correctamente.";
                    }
                }
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public bool Update(Cls_Especies obj, int rolId, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                using (var cn = _db.GetConnection())
                {
                    cn.Open();
                    using (var cmd = new SqlCommand("[SQM_CATALOGS].[USP_Update_Especies]", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@especieId", obj.EspecieId);
                        cmd.Parameters.AddWithValue("@nombreEspecie", obj.NombreEspecie);
                        cmd.Parameters.AddWithValue("@estadoId", obj.EstadoId);

                        cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();

                        resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                        mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
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
                    using (var cmd = new SqlCommand("[SQM_CATALOGS].[USP_Delete_Especies]", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@especieId", id);

                        cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();

                        resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                        mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                    }
                }
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return resultado;
        }
    }
}
