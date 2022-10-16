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
            this.view.onPost += postItem;
            this.view.onFindOne += findOne;
            this.view.onDelete += deleteItem;
            //this.view.startInteractionProcess();
        }

        public void setDataStorage(IDataStorage? dataStorage)
        {
            this.dataStorage = dataStorage;
            //printTable();
        }

        public bool init()
        {
            if (view == null || dataStorage == null) return false;
            viewItemsTable();
            view.init();
            return true;
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

        public void findOne()
        {
            if (view == null || dataStorage == null) return;

            int? id = view.getItemId();

            bool res = false;
            if (id != null)
            {
                var finded = dataStorage.getItemById(id.Value);
                if (finded != null)
                {
                    view.viewTable(new() { finded }, "Finded:");
                    res = true;
                }
            }
            if (!res) view.viewResult(false);
        }

        public void viewItemsTable()
        {
            if (view == null || dataStorage == null) return;

            var itemsList = dataStorage.getItems();

            view.viewTable(itemsList, "Items");
        }
    }
}