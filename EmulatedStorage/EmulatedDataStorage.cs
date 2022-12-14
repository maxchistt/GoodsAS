using Common.Interfaces;
using Common.Models;
using System.Data;

namespace EmulatedStorage
{
    public class EmulatedDataStorage : IDataStorage
    {
        private DataSet dataSet = new DataSet("EmulatedDatabase");
        private DataTable table;

        public EmulatedDataStorage()
        {
            table = dataSet.Tables.Add("Items");
            TableConverter.SetupTableColumns(table, typeof(Item));
            defaultFillTable();
        }

        private void defaultFillTable()
        {
            if (table == null) return;

            for (int i = 0; i < 5; i++)
            {
                Item item = new()
                {
                    Id = i,
                    Name = "Product" + i,
                    Description = "Description" + i,
                    Category = "ProductType" + i,
                    Cost = 100 + i
                };
                postItem(item);
            }
        }

        private static bool CheckPrimaryKeys(DataTable table)
        {
            return table.PrimaryKey.Count() > 0;
        }

        public Item? getItemById(int Id)
        {
            if (CheckPrimaryKeys(table))
            {
                var row = table.Rows.Find(Id);
                var item = row != null ? TableConverter.TryConvRowToItem<Item>(row) : null;
                return item;
            }
            return null;
        }

        public List<Item> getItems()
        {
            List<Item> list = new();
            foreach (DataRow row in table.Rows)
            {
                var item = TableConverter.TryConvRowToItem<Item>(row);
                if (item != null) list.Add(item);
            }
            return list;
        }

        public bool postItem(Item item)
        {
            var newRow = TableConverter.TryConvItemToRow<Item>(item, table);
            if (newRow != null)
            {
                if (CheckPrimaryKeys(table))
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
            if (CheckPrimaryKeys(table))
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