using Common.Models;
using GoodsAS_Console.Interfaces;
using System.Text.Json;

namespace GoodsAS_Console
{
    internal class ConsoleView : IView
    {
        public event Action onViewTable = () => { Console.WriteLine("Printing table:"); };

        public event Action onPost = () => { Console.WriteLine("Posting new item"); };

        public event Action onDelete = () => { Console.WriteLine("Deleting item"); };

        private string Title = "GoodsAS console app";

        public ConsoleView()
        {
            Console.WriteLine(Title + "\n");
            //startInteractionProcess();
        }

        public int? getItemId()
        {
            Console.WriteLine("Enter Id of item:");
            var input = Console.ReadLine();

            if (input == null) return null;

            int Num;

            return int.TryParse(input.Trim(), out Num) ? Num : null;
        }

        public Item? getItem()
        {
            Console.WriteLine("Fill item fields");
            Item? item = new();
            foreach (var prop in typeof(Item).GetProperties())
            {
                string name = prop.Name;
                Type type = prop.PropertyType;

                Console.WriteLine($"Enter property {name} with {type.Name} type:");
                var input = Console.ReadLine();
                if (input == null) return null;
                input = input.Trim().Normalize();

                if (type == typeof(int) || type == typeof(double) || type == typeof(float))
                {
                    double Num;

                    if (double.TryParse(input, out Num))
                    {
                        prop.SetValue(item, Convert.ChangeType(Num, prop.PropertyType));
                    }
                    else
                    {
                        item = null;
                        break;
                    }
                }
                else
                {
                    prop.SetValue(item, input);
                }
            }
            return item;
        }

        public void viewResult(bool res)
        {
            Console.WriteLine(res ? "Success\n" : "Error\n");
        }

        public void viewTable(List<Item> itemsList, string? tableName = null)
        {
            if (tableName != null) Console.WriteLine(tableName);
            Console.WriteLine("---");
            foreach (var item in itemsList)
            {
                Console.WriteLine(JsonSerializer.Serialize(item) + "\n---");
            }
            Console.WriteLine();
        }

        private enum Todo
        { close, post, print, delete }

        public void init()
        {
            writeInsruction();
            while (true)
            {
                var input = Console.ReadKey(true);

                int Num;

                if (int.TryParse(input.KeyChar.ToString(), out Num) && Enum.IsDefined(typeof(Todo), Num))
                {
                    switch ((Todo)Num)
                    {
                        case Todo.print:
                            onViewTable();
                            break;

                        case Todo.post:
                            onPost();
                            break;

                        case Todo.delete:
                            onDelete();
                            break;
                    }
                }
                else if (input.Key == ConsoleKey.Enter || input.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("Press Enter or Esc to close or any key to continue");
                    input = Console.ReadKey(true);
                    if (input.Key == ConsoleKey.Enter || input.Key == ConsoleKey.Escape) return;
                    Console.WriteLine();
                    writeInsruction();
                }
            }
        }

        private void writeInsruction()
        {
            string text = "Enter keys for next actions:\n";

            text += $"To post item      {(int)Todo.post}\n";
            text += $"To print table    {(int)Todo.print}\n";
            text += $"To delete item    {(int)Todo.delete}\n";
            text += $"To close          Esc\\Enter\n";

            Console.WriteLine(text);
        }
    }
}