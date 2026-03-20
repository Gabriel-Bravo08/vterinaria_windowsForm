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
    public class CD_Personal
    {
        private readonly CD_Connection _db = new CD_Connection();

        public List<Cls_Personal> SelectAll()
        {
            List<Cls_Personal> lista = new List<Cls_Personal>();
            using (var cn = _db.GetConnection())
            {
                cn.Open();
                using (var cmd = new SqlCommand("[SQM_SECURITY].[USP_SelectAll_Personal]", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Cls_Personal
                            {
                                PersonalId = Convert.ToInt32(dr["personalId"]),
                                PrimerNombre = dr["primerNombre"].ToString(),
                                SegundoNombre = dr["segundoNombre"].ToString(),
                                PrimerApellido = dr["primerApellido"].ToString(),
                                SegundoApellido = dr["segundoApellido"].ToString(),
                                Telefono = dr["telefono"].ToString(),
                                Email = dr["email"].ToString(),
                                NombreUsuario = dr["nombreUsuario"].ToString(),
                                RolId = Convert.ToInt32(dr["rolId"]),
                                EstadoId = Convert.ToInt32(dr["estadoId"]),
                                NombreRol = dr["nombreRol"].ToString(),
                                NombreEstado = dr["nombreEstado"].ToString()
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public string Create(Cls_Personal obj, int rolExecId, out int idGenerado)
        {
            idGenerado = 0;
            string mensaje = string.Empty;
            try
            {
                using (var cn = _db.GetConnection())
                {
                    cn.Open();
                    using (var cmd = new SqlCommand("[SQM_SECURITY].[USP_Create_Personal]", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EjecutadoPorRolId", rolExecId);
                        cmd.Parameters.AddWithValue("@primerNombre", obj.PrimerNombre);
                        cmd.Parameters.AddWithValue("@segundoNombre", obj.SegundoNombre ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@primerApellido", obj.PrimerApellido);
                        cmd.Parameters.AddWithValue("@segundoApellido", obj.SegundoApellido ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@telefono", obj.Telefono ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@email", obj.Email ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@nombreUsuario", obj.NombreUsuario);
                        cmd.Parameters.AddWithValue("@clave", obj.Clave);
                        cmd.Parameters.AddWithValue("@rolId", obj.RolId);
                        cmd.Parameters.AddWithValue("@estadoId", obj.EstadoId);
                        cmd.ExecuteNonQuery();
                        mensaje = "Personal creado correctamente.";
                    }
                }
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return mensaje;
        }

        public bool Update(Cls_Personal obj, int rolExecId, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                using (var cn = _db.GetConnection())
                {
                    cn.Open();
                    using (var cmd = new SqlCommand("[SQM_SECURITY].[USP_Update_Personal]", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EjecutadoPorRolId", rolExecId);
                        cmd.Parameters.AddWithValue("@personalId", obj.PersonalId);
                        cmd.Parameters.AddWithValue("@primerNombre", obj.PrimerNombre);
                        cmd.Parameters.AddWithValue("@segundoNombre", obj.SegundoNombre ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@primerApellido", obj.PrimerApellido);
                        cmd.Parameters.AddWithValue("@segundoApellido", obj.SegundoApellido ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@telefono", obj.Telefono ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@email", obj.Email ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@nombreUsuario", obj.NombreUsuario);
                        cmd.Parameters.AddWithValue("@clave", obj.Clave ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@rolId", obj.RolId);
                        cmd.Parameters.AddWithValue("@estadoId", obj.EstadoId);
                        resultado = cmd.ExecuteNonQuery() > 0;
                        if (resultado) mensaje = "Personal actualizado correctamente.";
                    }
                }
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return resultado;
        }

        public bool Delete(int id, int rolExecId, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                using (var cn = _db.GetConnection())
                {
                    cn.Open();
                    using (var cmd = new SqlCommand("[SQM_SECURITY].[USP_Delete_Personal]", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EjecutadoPorRolId", rolExecId);
                        cmd.Parameters.AddWithValue("@personalId", id);
                        resultado = cmd.ExecuteNonQuery() > 0;
                        if (resultado) mensaje = "Personal desactivado correctamente.";
                    }
                }
            }
            catch (Exception ex) { mensaje = ex.Message; }
            return resultado;
        }
    }
}
