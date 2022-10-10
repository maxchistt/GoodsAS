using EmulatedStorage;

namespace GoodsAS_Console
{
    internal class Program
    {
        private static string Title = "GoodsAS console app";

        private enum Todo
        { close, post, print, delete }

        private static void Main(string[] args)
        {
            Console.WriteLine(Title + "\n");

            DataOperations.setDataStorage(new EmulatedDataStorage());

            startInteractionProcess();
        }

        private static void startInteractionProcess()
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
                        case Todo.delete:
                            DataOperations.deleteItem();
                            break;

                        case Todo.print:
                            DataOperations.printTable();
                            break;

                        case Todo.post:
                            DataOperations.postItem();
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