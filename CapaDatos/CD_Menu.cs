using CapaEntidad;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace CapaDatos
{
    public class CD_Menu
    {
        private readonly CD_Connection _db = new CD_Connection();

        public List<Cls_Menus> ObtenerMenusPorRol(int rolId)
        {
            List<Cls_Menus> listMenus = new List<Cls_Menus>();
            using (var connection = _db.GetConnection())
            {
                connection.Open();
                using (var cmd = new SqlCommand("[SQM_SECURITY].[USP_ObtenerMenusPorRol]", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RolId", rolId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listMenus.Add(new Cls_Menus
                            {
                                MenuId = Convert.ToInt32(reader["menuId"]),
                                Nombre = reader["nombre"].ToString(),
                                Descripcion = reader["descripcion"].ToString()
                            });
                        }
                    }
                }
            }
            return listMenus;
        }
    }
}
