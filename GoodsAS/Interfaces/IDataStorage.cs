using GoodsAS.Models;

namespace GoodsAS.Interfaces
{
    internal interface IDataStorage
    {
        Item? getItemById(int Id);

        List<Item> getItems();

        bool postItem(Item item);

        bool deleteItem(int Id);
    }
}