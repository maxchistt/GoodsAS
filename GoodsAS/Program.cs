using EmulatedStorage;

namespace GoodsAS_Console
{
    internal class Program
    {
        private static string Title = "GoodsAS console app";

        private enum Todo
        { close, post, print, delete }

        private static DataOperations? controller;

        private static void Main(string[] args)
        {
            controller = new DataOperations();

            Console.WriteLine(Title + "\n");

            controller.setDataStorage(new EmulatedDataStorage());

            startInteractionProcess();
        }

        private static void startInteractionProcess()
        {
            writeInsruction();
            while (true)
            {
                var input = Console.ReadKey(true);

                int Num;
                bool isNum = int.TryParse(input.KeyChar.ToString(), out Num);

                if (isNum)
                {
                    bool exists = Enum.IsDefined(typeof(Todo), Num);
                    if (exists && controller != null)
                    {
                        Todo todo = (Todo)Num;
                        switch (todo)
                        {
                            case Todo.delete:
                                controller.deleteItem();
                                break;

                            case Todo.print:
                                controller.printTable();
                                break;

                            case Todo.post:
                                controller.postItem();
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
            text += $"To print table    {(int)Todo.print}\n";
            text += $"To delete item    {(int)Todo.delete}\n";
            text += $"To close          Esc\\Enter\n";

            Console.WriteLine(text);
        }
    }
}