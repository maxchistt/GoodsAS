using Common.Interfaces;
using Common.Models;

namespace GoodsAS_Console
{
    internal class Contoller
    {
        private IDataStorage? dataStorage;
        private ConsoleView? view;

        public Contoller()
        { }

        public Contoller(IDataStorage? dataStorage, ConsoleView view)
        {
            setDataStorage(dataStorage);
            setView(view);
        }

        public void setView(ConsoleView view)
        {
            this.view = view;
            this.view.onPrint += printItemsTable;
            this.view.onPost += deleteItem;
            this.view.onDelete += postItem;
            //this.view.startInteractionProcess();
        }

        public void setDataStorage(IDataStorage? dataStorage)
        {
            this.dataStorage = dataStorage;
            //defaultFillTable();
            //printTable();
        }

        public bool start()
        {
            if (view == null || dataStorage == null) return false;
            defaultFillTable();
            printItemsTable();
            view.startInteractionProcess();
            return true;
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
            if (view == null || dataStorage == null) return;

            int? id = view.getItemId();

            bool res = false;
            if (id != null)
            {
                res = dataStorage.deleteItem(id.Value);
            }
            view.displayResult(res);
        }

        public void postItem()
        {
            if (view == null || dataStorage == null) return;

            var item = view.getItem();

            bool res = item != null ? dataStorage.postItem(item) : false;

            view.displayResult(res);
        }

        public void printItemsTable()
        {
            if (view == null || dataStorage == null) return;

            var itemsList = dataStorage.getItems();

            view.printTable(itemsList, "Items");
        }
    }
}