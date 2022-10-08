using GoodsAS.Interfaces;
using GoodsAS.Models;
using System.Data;
using System.Reflection;

namespace GoodsAS.Storage
{
    internal class EmulatedStorage : IDataStorage
    {
        private DataTable table = new DataTable("Goods");

        public EmulatedStorage()
        {
            SetupTableColumns();
        }

        private void SetupTableColumns()
        {
            var primaryKeys = new List<DataColumn>();
            foreach (var prop in typeof(Item).GetProperties())
            {
                var name = prop.Name;
                var type = prop.PropertyType;
                var col = table.Columns.Add(name, type);
                if ((name == "Id" || name == "id") && type == typeof(int)) primaryKeys.Add(col);
            }
            table.PrimaryKey = primaryKeys.ToArray();
        }

        private bool CheckPrimaryKeys(ref DataTable table)
        {
            return table.PrimaryKey.Count() > 0;
        }

        private bool CheckСomparability(Type type, ref DataTable table)
        {
            var cols = table.Columns;

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

        private DataRow? ConvItemToRow(Item item)
        {
            if (!CheckСomparability(typeof(Item), ref table)) return null;

            DataRow? row = table.NewRow();

            foreach (DataColumn col in row.Table.Columns)
            {
                var prop = item.GetType().GetProperty(col.ColumnName);
                if (prop != null) row[col.ColumnName] = prop.GetValue(item, null);
            }

            return row;
        }

        private Item? ConvRowToItem(DataRow row)
        {
            if (!CheckСomparability(typeof(Item), ref table)) return null;

            Item? item = new();

            foreach (PropertyInfo prop in typeof(Item).GetProperties())
            {
                var col = row.Table.Columns[prop.Name];
                if (col != null) prop.SetValue(item, row[col.ColumnName]);
            }

            return item;
        }

        public Item? getItemById(int Id)
        {
            if (CheckPrimaryKeys(ref table))
            {
                var row = table.Rows.Find(Id);
                var item = row != null ? ConvRowToItem(row) : null;
                return item;
            }
            return null;
        }

        public List<Item> getItems()
        {
            List<Item> list = new();
            foreach (DataRow row in table.Rows)
            {
                var item = ConvRowToItem(row);
                if (item != null) list.Add(item);
            }
            return list;
        }

        public bool postItem(Item item)
        {
            var newRow = ConvItemToRow(item);
            if (newRow != null)
            {
                if (CheckPrimaryKeys(ref table))
                {
                    var exitingRow = table.Rows.Find(item.Id);
                    if (exitingRow != null)
                    {
                        // if exiting
                        int index = table.Rows.IndexOf(exitingRow);
                        table.Rows.RemoveAt(index);
                        table.Rows.InsertAt(newRow, index);
                    }
                    else
                    {
                        // if not exiting
                        table.Rows.Add(newRow);
                    }
                }
                else
                {
                    table.Rows.Add(newRow);
                }
                return true;
            }
            return false;
        }

        public bool deleteItem(int Id)
        {
            if (CheckPrimaryKeys(ref table))
            {
                var row = table.Rows.Find(Id);
                if (row != null)
                {
                    table.Rows.Remove(row);
                    return true;
                }
            }
            return false;
        }
    }
}