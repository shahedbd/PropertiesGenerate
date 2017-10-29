using PropertiesGen.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace PropertiesGen
{
    class Program
    {
        private static string TableName { get; set; }
        private static List<TableSchema> _tableSchema;
        private static string currentPath = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())) + "\\OutPut\\";

        static void Main(string[] args)
        {
            //SingleTableWiseByInput();
            AllTables();
        }


        static void SingleTableWiseByInput()
        {
            while (true)
            {
                Console.Write("Insert table name (type \'q\' to exit): ");
                TableName = Console.ReadLine();

                if ("q".Equals(TableName))
                {
                    break;
                }

                _tableSchema = Helper.GetTableSchema(TableName);
                if (null == _tableSchema)
                {
                    Console.WriteLine("Invalid table name");
                    TableName = "";
                    continue;
                }

                PropertiesCreation _PropertiesCreation = new PropertiesCreation(TableName, _tableSchema, currentPath);
                _PropertiesCreation.WriteModel();

                Process.Start("notepad.exe", currentPath + TableName + ".cs");
            }
        }

        static void AllTables()
        {
            List<string> tableNameList = Helper.GetDBTablesNameList();

            foreach (var tableName in tableNameList)
            {
                var _tableSchema = Helper.GetTableSchema(tableName);
                PropertiesCreation _PropertiesCreation = new PropertiesCreation(tableName, _tableSchema, currentPath);
                _PropertiesCreation.WriteModel();

            }

            Process.Start("explorer.exe", currentPath);
        }

    }
}
