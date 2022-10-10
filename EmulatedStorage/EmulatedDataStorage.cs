using Common.Interfaces;
using Common.Models;
using System.Data;
using System.Reflection;

namespace EmulatedStorage
{
    public class EmulatedDataStorage : IDataStorage
    {
        private DataTable table = new DataTable("Goods");

        public EmulatedDataStorage()
        {
            TableColsSetter.SetupTableColumns(ref table, typeof(Item));
        }

        private static bool CheckPrimaryKeys(in DataTable table)
        {
            return table.PrimaryKey.Count() > 0;
        }

        public Item? getItemById(int Id)
        {
            if (CheckPrimaryKeys(in table))
            {
                var row = table.Rows.Find(Id);
                var item = row != null ? TableRowConverter.ConvRowToItem(row) : null;
                return item;
            }
            return null;
        }

        public List<Item> getItems()
        {
            List<Item> list = new();
            foreach (DataRow row in table.Rows)
            {
                var item = TableRowConverter.ConvRowToItem(row);
                if (item != null) list.Add(item);
            }
            return list;
        }

        public bool postItem(Item item)
        {
            var newRow = TableRowConverter.ConvItemToRow(item, in table);
            if (newRow != null)
            {
                if (CheckPrimaryKeys(in table))
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
            if (CheckPrimaryKeys(in table))
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