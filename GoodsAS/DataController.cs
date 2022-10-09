using Common.Interfaces;
using Common.Models;
using System.Text.Json;

namespace GoodsAS_Console
{
    internal class DataController
    {
        private IDataStorage? dataStorage;

        public DataController() { }

        public DataController(IDataStorage? dataStorage)
        {
            setDataStorage(dataStorage);
        }

        public void setDataStorage(IDataStorage? dataStorage)
        {
            this.dataStorage = dataStorage;
            defaultFillTable();
            readTable();
        }

        private void defaultFillTable()
        {
            if (dataStorage == null) return;

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
                dataStorage.postItem(item);
            }
        }

        public void deleteItem()
        {
            if (dataStorage == null) return;

            Console.WriteLine("Deleting item");
            Console.WriteLine("Enter Id to delete:");
            var input = Console.ReadLine();
            if (input == null) return;

            int Num;
            bool isNum = int.TryParse(input.Trim(), out Num);

            if (isNum)
            {
                bool res = dataStorage.deleteItem(Num);
                Console.WriteLine(res ? "Deleted" : "Error");
            }
            Console.WriteLine();
        }

        public void postItem()
        {
            if (dataStorage == null) return;

            Console.WriteLine("Posting new item");

            Item item = new();
            bool itemBuildRes = true;
            foreach (var prop in typeof(Item).GetProperties())
            {
                string name = prop.Name;
                Type type = prop.PropertyType;

                Console.WriteLine($"Enter property {name} with {type.Name} type:");
                var input = Console.ReadLine();
                if (input == null) return;
                input = input.Trim().Normalize();

                if (type == typeof(int) || type == typeof(double) || type == typeof(float))
                {
                    double Num;
                    bool isNum = double.TryParse(input, out Num);

                    if (isNum)
                    {
                        prop.SetValue(item, Convert.ChangeType(Num, prop.PropertyType));
                    }
                    else
                    {
                        itemBuildRes = false;
                        break;
                    }
                }
                else
                {
                    prop.SetValue(item, input);
                }
            }
            bool res = itemBuildRes ? dataStorage.postItem(item) : false;
            Console.WriteLine(res ? "Posted" : "Error");
            Console.WriteLine();
        }

        public void readTable()
        {
            if (dataStorage == null) return;

            Console.WriteLine("Printing table:");
            var res = dataStorage.getItems();
            Console.WriteLine("---");
            foreach (var item in res)
            {
                Console.WriteLine(JsonSerializer.Serialize(item) + "\n---");
            }
            Console.WriteLine();
        }
    }
}