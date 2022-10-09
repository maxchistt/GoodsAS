using Common.Models;

namespace Common.Interfaces
{
    public interface IDataStorage
    {
        Item? getItemById(int Id);

        List<Item> getItems();

        bool postItem(Item item);

        bool deleteItem(int Id);
    }
}