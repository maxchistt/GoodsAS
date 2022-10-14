using System.Data;
using System.Reflection;

namespace EmulatedStorage
{
    internal static class TableConverter
    {
        public static void SetupTableColumns(DataTable table, Type modelType)
        {
            table.Columns.Clear();
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

        public static bool CheckСomparability(Type type, DataTable table)
        {
            if (table.Columns.Count == type.GetProperties().Count())
            {
                foreach (DataColumn col in table.Columns)
                {
                    var prop = type.GetProperty(col.ColumnName);
                    if (prop == null || prop.PropertyType != col.DataType) return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public static DataRow? ConvItemToRow<T>(T item, DataTable table)
        {
            if (item == null || !TableConverter.CheckСomparability(typeof(T), table)) return null;

            DataRow? row = table.NewRow();

            foreach (DataColumn col in row.Table.Columns)
            {
                var prop = item.GetType().GetProperty(col.ColumnName);
                if (prop != null) row[col.ColumnName] = prop.GetValue(item, null);
            }

            return row;
        }

        public static T? ConvRowToItem<T>(DataRow row) where T : class, new()
        {
            if (!TableConverter.CheckСomparability(typeof(T), row.Table)) return null;

            T? item = new T();

            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                var col = row.Table.Columns[prop.Name];
                if (col != null) prop.SetValue(item, row[col.ColumnName]);
            }

            return item;
        }
    }
}