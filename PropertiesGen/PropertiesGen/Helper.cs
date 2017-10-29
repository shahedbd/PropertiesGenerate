using PropertiesGen.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace PropertiesGen
{
    public static class Helper
    {
        public static List<TableSchema> GetTableSchema(string TableName)
        {
            SqlConnection connection = MSSQLConn.MSSQLConnection();
            try
            {
                var sql = "select top 0 * from " + TableName;
                connection.Open();
                var cmd = new SqlCommand(sql, connection);
                var reader = cmd.ExecuteReader();

                var schemaTable = reader.GetSchemaTable();

                if (schemaTable != null)
                    return (from DataRow row in schemaTable.Rows
                            select new TableSchema
                            {
                                ColumnName = row["ColumnName"].ToString(),
                                ColumnSize = row["ColumnSize"].ToString(),
                                DataTypeName = ConvertDBToBackEndType(row["DataTypeName"].ToString()),
                                DbTypeName = row["DataTypeName"].ToString(),
                                IsIdentity = row["IsIdentity"].ToString()
                            }).ToList();

                connection.Close();
            }
            catch (Exception)
            {
                return null;
            }
            return new List<TableSchema>();
        }

        private static string ConvertDBToBackEndType(string sqlDataType)
        {
            switch (sqlDataType)
            {
                case "bigint":
                    return "long";
                case "nvarchar":
                    return "string";
                case "nchar":
                    return "string";
                case "varbinary":
                    return "string";



                case "datetime":
                    return "DateTime";
                case "bit":
                    return "bool";
                case "int":
                    return "int";
                case "tinyint":
                    return "tinyint";
                case "decimal":
                    return "decimal";
            }
            return "";
        }


        public static List<string> GetDBTablesNameList()
        {
            List<string> tblNamesList = new List<string>();
            DataTable dt = new DataTable();
            SqlConnection connection = MSSQLConn.MSSQLConnection();
            try
            {
                connection.Open();
                SqlCommand sqlCMD = new SqlCommand("select name from sys.tables", connection);
                SqlDataReader reader = sqlCMD.ExecuteReader();
                dt.Load(reader);

                foreach (DataRow row in dt.Rows)
                {
                    tblNamesList.Add(row[0].ToString());
                }

                connection.Close();
                return tblNamesList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }



    }
}
