using Common.Interfaces;
using Common.Models;
using EmulatedStorage;
using System.Security.AccessControl;
using System.Text.Json;

namespace GoodsAS_Console
{
    internal class Program
    {
        private static string Title = "GoodsAS console app";

        private enum Todo
        { close, post, read, delete }
        
        private static IDataStorage? dataStorage;

        private static void Main(string[] args)
        {
            dataStorage = new EmulatedDataStorage();

            Console.WriteLine(Title + "\n");

            defaultFillTable();
            readTable();
            writeInsruction();

            while (true)
            {
                var input = Console.ReadKey(true);

                int Num;
                bool isNum = int.TryParse(input.KeyChar.ToString(), out Num);

                if (isNum)
                {
                    bool exists = Enum.IsDefined(typeof(Todo), Num);
                    if (exists)
                    {
                        Todo todo = (Todo)Num;
                        switch (todo)
                        {
                            case Todo.delete:
                                deleteItem();
                                break;

                            case Todo.read:
                                readTable();
                                break;

                            case Todo.post:
                                postItem();
                                break;
                        }
                    }
                    else continue;
                }
                else if (input.Key == ConsoleKey.Enter || input.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("Press Enter or Esc to close or any key to continue");
                    input = Console.ReadKey(true);
                    if (input.Key == ConsoleKey.Enter || input.Key == ConsoleKey.Escape) return;
                    Console.WriteLine();
                    writeInsruction();
                    continue;
                }
                else continue;
            }
        }

        private static void writeInsruction()
        {
            string text = "Enter keys for next actions:\n";

            text += $"To post item      {(int)Todo.post}\n";
            text += $"To read table     {(int)Todo.read}\n";
            text += $"To delete item    {(int)Todo.delete}\n";
            text += $"To close          Esc\\Enter\n";

            Console.WriteLine(text);
        }

        private static void deleteItem()
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

        private static void postItem()
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

        private static void readTable()
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

        private static void defaultFillTable()
        {
            if (dataStorage == null) return;

            for (int i = 0; i < 5; i++)
            {
                Item item = new(i, "Product" + i, "Description" + i, "ProductType" + i, 100 + i);
                dataStorage.postItem(item);
            }
        }
    }
}