using Common.Models;
using System.Data;
using System.Reflection;

namespace EmulatedStorage
{
    internal static class TableRowConverter
    {
        public static bool CheckСomparability(Type type, in DataTable table)
        {
            return CheckСomparability(type, table.Columns);
        }

        public static bool CheckСomparability(Type type, DataColumnCollection cols)
        {
            if (cols.Count == type.GetProperties().Count())
            {
                foreach (DataColumn col in cols)
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

        public static DataRow? ConvItemToRow(Item item, in DataTable table)
        {
            if (!TableRowConverter.CheckСomparability(typeof(Item), table.Columns)) return null;

            DataRow? row = table.NewRow();

            foreach (DataColumn col in row.Table.Columns)
            {
                var prop = item.GetType().GetProperty(col.ColumnName);
                if (prop != null) row[col.ColumnName] = prop.GetValue(item, null);
            }

            return row;
        }

        public static Item? ConvRowToItem(DataRow row)
        {
            if (!TableRowConverter.CheckСomparability(typeof(Item), row.Table.Columns)) return null;

            Item? item = new();

            foreach (PropertyInfo prop in typeof(Item).GetProperties())
            {
                var col = row.Table.Columns[prop.Name];
                if (col != null) prop.SetValue(item, row[col.ColumnName]);
            }

            return item;
        }
    }
}