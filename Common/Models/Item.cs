namespace Common.Models
{
    public class Item
    {
        public Item()
        {
            Id = 0;
            Name = "";
            Description = "";
            Category = "";
            Cost = 0;
        }

        public Item(string Name, string Description, string Category, float Cost)
        {
            this.Name = Name;
            this.Description = Description;
            this.Category = Category;
            this.Cost = Cost;
        }

        public Item(int Id, string Name, string Description, string Category, float Cost) : this(Name, Description, Category, Cost)
        {
            this.Id = Id;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public float Cost { get; set; }
    }
}