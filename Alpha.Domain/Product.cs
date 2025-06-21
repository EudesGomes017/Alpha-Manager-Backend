using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alpha.Domain
{
    public class Product : EntityBase
    {
        public string Name { get;  set; }
        public decimal Price { get;  set; }
        public string Barcode { get;  set; }
        public string ImageBytes { get;  set; }


        public Product() { }

        public Product(string name, decimal price, string barcode, string imageBytes, DateTime createdAt)
        {
            Name = name;
            Price = price;
            Barcode = barcode;
            ImageBytes = imageBytes;
            CreatedAt = createdAt;
        }

        public void Update(string name, decimal price, string barcode, string imageBytes)
        {
            Name = name;
            Price = price;
            Barcode = barcode;
            ImageBytes = imageBytes;
        }
    }
}
