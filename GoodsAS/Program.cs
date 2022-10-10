using EmulatedStorage;

namespace GoodsAS_Console
{
    internal class Program
    {
        private static Contoller? controller;

        private static void Main(string[] args)
        {
            controller = new Contoller();

            controller.setDataStorage(new EmulatedDataStorage());
            controller.setView(new ConsoleView());
            controller.start();
        }
    }
}