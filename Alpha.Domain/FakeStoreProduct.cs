using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Alpha.Domain
{

    public class FakeStoreProduct : EntityBase
    {
        public string Title { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }




        public FakeStoreProduct(string title, double price, string description, string category, string image)
        {
            
            Title = title;
            Price = price;
            Description = description;
            Image = image;
        }
    }
}
