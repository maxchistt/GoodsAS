using Common.Models;

namespace GoodsAS_Console.Interfaces
{
    internal interface IView
    {
        event Action onDelete;
        event Action onPost;
        event Action onViewTable;

        Item? getItem();
        int? getItemId();
        void init();
        void viewResult(bool res);
        void viewTable(List<Item> itemsList, string? tableName = null);
    }
}