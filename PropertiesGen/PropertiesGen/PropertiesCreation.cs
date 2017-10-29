using PropertiesGen.Entities;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace PropertiesGen
{
    internal class PropertiesCreation
    {
        private readonly string _tableName;
        private readonly List<TableSchema> _tableSchema;
        private readonly TableSchema _tablePk;
        private static string strOutPutPath;

        public PropertiesCreation(string tableName, List<TableSchema> tableSchema, string path)
        {
            _tableSchema = tableSchema;
            var firstOrDefault = tableSchema.FirstOrDefault(p => p.IsIdentity.ToLower() == "true");
            if (firstOrDefault != null)
                _tablePk = firstOrDefault;
            _tableName = tableName;

            strOutPutPath = path;
            if (!Directory.Exists(strOutPutPath))  //if it doesn't exist, create
                Directory.CreateDirectory(strOutPutPath);
        }

        public void WriteModel()
        {
            using (var writer = new StreamWriter(strOutPutPath + "\\" + _tableName + ".cs"))
            {

                writer.WriteLine("using System;\n");
                writer.WriteLine("namespace XYZProject");
                writer.WriteLine("{");
                writer.WriteLine("    public class " + _tableName);
                writer.WriteLine("    {");

                foreach (var schema in _tableSchema)
                {
                    if ("ntext".Equals(schema.DbTypeName))
                    {
                        writer.WriteLine("      public string " + schema.ColumnName + " { get; set; }");
                        continue;
                    }
                    writer.WriteLine("      public " + schema.DataTypeName + " " + schema.ColumnName + " { get; set; }");
                }

                writer.WriteLine("    }");
                writer.WriteLine("}");
            }
        }
    }
}
