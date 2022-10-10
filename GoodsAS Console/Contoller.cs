using Common.Interfaces;
using Common.Models;
using GoodsAS_Console.Interfaces;

namespace GoodsAS_Console
{
    internal class Contoller
    {
        private IDataStorage? dataStorage;
        private IView? view;

        public Contoller()
        { }

        public Contoller(IDataStorage? dataStorage, IView view)
        {
            setDataStorage(dataStorage);
            setView(view);
        }

        public void setView(IView view)
        {
            this.view = view;
            this.view.onViewTable += viewItemsTable;
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

        public bool init()
        {
            if (view == null || dataStorage == null) return false;
            defaultFillTable();
            viewItemsTable();
            view.init();
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
            view.viewResult(res);
        }

        public void postItem()
        {
            if (view == null || dataStorage == null) return;

            var item = view.getItem();

            bool res = item != null ? dataStorage.postItem(item) : false;

            view.viewResult(res);
        }

        public void viewItemsTable()
        {
            if (view == null || dataStorage == null) return;

            var itemsList = dataStorage.getItems();

            view.viewTable(itemsList, "Items");
        }
    }
}