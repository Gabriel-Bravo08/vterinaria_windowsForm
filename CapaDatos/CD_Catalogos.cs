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
    public class CD_Catalogos
    {
        private readonly CD_Connection _db = new CD_Connection();

        public List<Cls_Especies> ListarEspecies()
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
                                NombreEspecie = dr["nombreEspecie"].ToString()
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public List<Cls_Razas> ListarRazas(int especieId = 0)
        {
            List<Cls_Razas> lista = new List<Cls_Razas>();
            using (var cn = _db.GetConnection())
            {
                cn.Open();
                string sql = "[SQM_CATALOGS].[USP_SelectAll_Razas]";
                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Cls_Razas
                            {
                                RazaId = Convert.ToInt32(dr["razaId"]),
                                EspecieId = Convert.ToInt32(dr["especieId"]),
                                NombreRaza = dr["nombreRaza"].ToString()
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public List<Cls_Especialidades> ListarEspecialidades()
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
                                NombreEspecialidad = dr["nombreEspecialidad"].ToString()
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public List<Cls_Roles> ListarRoles()
        {
            List<Cls_Roles> lista = new List<Cls_Roles>();
            using (var cn = _db.GetConnection())
            {
                cn.Open();
                using (var cmd = new SqlCommand("[SQM_SECURITY].[USP_SelectRoles]", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Cls_Roles
                            {
                                RolId = Convert.ToInt32(dr["rolId"]),
                                NombreRol = dr["nombreRol"].ToString()
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public List<Cls_Personal> ListarVeterinarios()
        {
            List<Cls_Personal> lista = new List<Cls_Personal>();
            using (var cn = _db.GetConnection())
            {
                cn.Open();
                using (var cmd = new SqlCommand("[SQM_SECURITY].[USP_SelectVeterinarios]", cn))
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
                                PrimerApellido = dr["primerApellido"].ToString()
                            });
                        }
                    }
                }
            }
            return lista;
        }
    }
}
