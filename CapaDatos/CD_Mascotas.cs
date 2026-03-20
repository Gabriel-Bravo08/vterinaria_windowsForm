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
    public class CD_Mascotas
    {
        private readonly CD_Connection _db = new CD_Connection();

        public List<Cls_Mascotas> SelectAll()
        {
            List<Cls_Mascotas> lista = new List<Cls_Mascotas>();
            using (var cn = _db.GetConnection())
            {
                cn.Open();
                using (var cmd = new SqlCommand("[SQM_CORE].[USP_SelectAll_Mascotas]", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Cls_Mascotas
                            {
                                MascotaId = Convert.ToInt32(dr["mascotaId"]),
                                ClienteId = Convert.ToInt32(dr["clienteId"]),
                                EspecieId = Convert.ToInt32(dr["especieId"]),
                                NombreMascota = dr["nombreMascota"].ToString(),
                                FechaNacimiento = dr["fechaNacimiento"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr["fechaNacimiento"]),
                                Peso = dr["peso"] == DBNull.Value ? null : (decimal?)Convert.ToDecimal(dr["peso"]),
                                Color = dr["color"].ToString(),
                                EstadoId = Convert.ToInt32(dr["estadoId"]),
                                NombreCliente = dr["nombreCliente"].ToString(),
                                NombreEspecie = dr["nombreEspecie"].ToString(),
                                NombreEstado = dr["nombreEstado"].ToString()
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public string Create(Cls_Mascotas obj, int rolId, out int idGenerado)
        {
            idGenerado = 0;
            string mensaje = string.Empty;
            try
            {
                using (var cn = _db.GetConnection())
                {
                    cn.Open();
                    using (var cmd = new SqlCommand("[SQM_CORE].[USP_Create_Mascotas]", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EjecutadoPorRolId", rolId);
                        cmd.Parameters.AddWithValue("@clienteId", obj.ClienteId);
                        cmd.Parameters.AddWithValue("@especieId", obj.EspecieId);
                        cmd.Parameters.AddWithValue("@nombreMascota", obj.NombreMascota);
                        cmd.Parameters.AddWithValue("@fechaNacimiento", obj.FechaNacimiento ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@peso", obj.Peso ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@color", obj.Color ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@estadoId", obj.EstadoId);

                        cmd.ExecuteNonQuery();
                        mensaje = "Mascota creada correctamente.";
                    }
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return mensaje;
        }

        public bool Update(Cls_Mascotas obj, int rolId, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                using (var cn = _db.GetConnection())
                {
                    cn.Open();
                    using (var cmd = new SqlCommand("[SQM_CORE].[USP_Update_Mascotas]", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EjecutadoPorRolId", rolId);
                        cmd.Parameters.AddWithValue("@mascotaId", obj.MascotaId);
                        cmd.Parameters.AddWithValue("@clienteId", obj.ClienteId);
                        cmd.Parameters.AddWithValue("@especieId", obj.EspecieId);
                        cmd.Parameters.AddWithValue("@nombreMascota", obj.NombreMascota);
                        cmd.Parameters.AddWithValue("@fechaNacimiento", obj.FechaNacimiento ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@peso", obj.Peso ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@color", obj.Color ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@estadoId", obj.EstadoId);

                        resultado = cmd.ExecuteNonQuery() > 0;
                        if (resultado) mensaje = "Mascota actualizada correctamente.";
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
                    using (var cmd = new SqlCommand("[SQM_CORE].[USP_Delete_Mascotas]", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EjecutadoPorRolId", rolId);
                        cmd.Parameters.AddWithValue("@mascotaId", id);

                        resultado = cmd.ExecuteNonQuery() > 0;
                        if (resultado) mensaje = "Mascota desactivada correctamente.";
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
