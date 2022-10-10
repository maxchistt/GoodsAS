using System.Data;

namespace EmulatedStorage
{
    internal static class TableColsSetter
    {
        public static void SetupTableColumns(ref DataTable table, Type modelType)
        {
            var primaryKeys = new List<DataColumn>();
            foreach (var prop in modelType.GetProperties())
            {
                var name = prop.Name;
                var type = prop.PropertyType;
                var col = table.Columns.Add(name, type);
                if ((name == "Id" || name == "id") && type == typeof(int)) primaryKeys.Add(col);
            }
            table.PrimaryKey = primaryKeys.ToArray();
        }
    }
}