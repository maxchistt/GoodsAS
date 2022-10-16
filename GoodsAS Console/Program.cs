using EmulatedStorage;
using DatabaseStorageSQL;

namespace GoodsAS_Console
{
    internal class Program
    {
        const bool realDb = true;
        private static Contoller? controller;

        private static void Main(string[] args)
        {
            controller = new Contoller();

            controller.setDataStorage(realDb ? new DbStorageSQL() : new EmulatedDataStorage());
            controller.setView(new ConsoleView());
            controller.init();
        }
    }
}