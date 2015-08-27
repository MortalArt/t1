namespace Store.Models
{
    public class Item
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Image { set; get; }
        public int  Price { set; get; }
        public int? CategoryId { set; get; }
        public Category Category { get; set; }
    }
    public class Category
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public int ParentId { set; get; }
    }
}